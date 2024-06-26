// Application.cs
//
// Implements the Application class, the central manager and authority on game logic.
using Godot;
using System;
using System.Collections.Concurrent;
using System.Text;



public class Application : Node
{

public const int gridHeight = 15;
public const int gridWidth = 41;
const int mapHeight = 34;
const int mapWidth = 102;


// The state in which the game (should be) at.
// Both are waiting for user input; the difference is what the input should address.
public enum State {
	WAIT_CHOICE_NODE,
	WAIT_CHOICE_EVENT
}
// No body for neither the constructor nor the destructor
//public Application() {}
//~Application() {}


// Given the file path to the map.txt, return the concatenated 
// string representation of the map.
public string readMapFile(string mapFilePath) {
	File file = new File();
	file.Open(mapFilePath, File.ModeFlags.Read);
	string text = file.GetAsText();
	file.Close();

	return text;
}


// Initializes game resources; calls appropriate functions to read from files and initialize nodes, events, and map.
public override void _Ready() {
	// call files...
	// Init Nodes
	// Init Events
	// read the map!

	// ========== Begin Loading ==========  

	try {
		m_state = State.WAIT_CHOICE_EVENT;
		
		m_node = GetNode("UIManager") as Godot.Object;
		
		string fileContents = readMapFile("res://data/map.txt");
		
		int index = 0;
		m_map = new char[mapHeight, mapWidth];
		for (int row = 0; row < mapHeight; ++row) {
			for (int col = 0; col < mapWidth; ++col) {
				m_map[row, col] = fileContents[index];
				++index;
			}
			++index;
	}
	
	// Load events
	Json json_node = GetNode("JSONController") as Json;
	m_events = json_node.LoadEvents("res://data/events.json", out int numEvents, out int numWeighted);
	m_nodes = json_node.LoadSeaNodes("res://data/nodes.json", out int numNodes);
	
	// Load event weights
	m_eventWeights = new float[numWeighted];
	m_validRandomEvents = new int[numWeighted];
	m_totalWeight = 0.0f;

	for (int i = 0, j = 0; i < numEvents; ++i) {
		if (m_events[i].Probability > 0) {
			GD.Print("Event title \"", m_events[i].Title, "\" added to randomValidEvents with weight ", m_totalWeight);

			m_eventWeights[j] = m_totalWeight;
			m_totalWeight += m_events[i].Probability;
			m_validRandomEvents[j] = i;
			++j;
		}
	}
	
	// Seed randomization
	m_random = new Random();

	// ========== End Loading ==========  
	
	// Start the game!
	StartGame();
	
	}
	catch(Exception e) {GD.Print(e);}
}

int RandomEvent() {
	float randomNumber = (float) m_random.NextDouble() * m_totalWeight;
	int eventIndex = 0;
	while (eventIndex < m_eventWeights.Length - 1 && m_eventWeights[eventIndex + 1] < randomNumber) {
		++eventIndex;
	}
	return m_validRandomEvents[eventIndex];
}

// UserInput(int)
// Handles the given user input, received as an integer input.
// =============================================
// input:	User's input.
public void UserInput(int input) {
	if (m_state == State.WAIT_CHOICE_NODE) {
		HandleNodeChoice(input - 1);
	}
	else if (m_state == State.WAIT_CHOICE_EVENT) {
		HandleEventChoice(input - 1);
	}
}


// HandleNodeChoice(int)
// Handles the given user choice, received as an integer input.
// Invalid choices will be ignored.
// =============================================
// choice:	User's choice.
private void HandleNodeChoice(int choice) {
	if (choice < 0) {
		return;
	}

	int[] adjList = m_nodes[m_nodeId].AdjList; 
	if (choice < adjList.Length) {
		TravelEdge(adjList[choice]);
	}
	// option after travel is rest
	else if (choice == adjList.Length) {
		Health += 10;
		if (Health > 100) {
			Health = 100;
		}
	}
	// option after rest is resupply
	else if (choice > adjList.Length) {
		if (Gold < 10) {
			m_node.Call("draw_event", 
						"You're poor!", 
						"You do not have enough money for this.", 
						new string[]{ "Okay." },
						"null");

			m_state = State.WAIT_CHOICE_EVENT;
			m_eventId = -1; // signifies that we're just waiting for a response. any response.
			return;
		}
		Food += 10;
		m_node.Call("gain_food_effect");
		Gold -= 10;
		m_node.Call("lose_gold_effect");
		
		if (Food > 100) {
			Food = 100;
		}
	}
}


// TravelEdge(int)
// Travels to the desired SeaNode.
// A random event will be triggered.
// =============================================
// choice:    User's choice.
private void TravelEdge(int destination) {
	m_nodeId = destination;
	Food -= 5;
	if (Food < 0) { Food = 0; }
	m_node.Call("lose_food_effect");

	
	m_nodes[destination].Visit();
	m_map[m_nodes[destination].Row, m_nodes[destination].Col] = '@';

	int triggered = RandomEvent();
	Event triggeredEvent = m_events[triggered];

	Food += triggeredEvent.DeltaFood;
	if (triggeredEvent.DeltaFood < 0) {
		m_node.Call("lose_food_effect");
	}
	else if (triggeredEvent.DeltaFood > 0) {
		m_node.Call("gain_food_effect");
	}
	if (Food < 0) {
		Food = 0;
	}

	Gold += triggeredEvent.DeltaGold;
	if (triggeredEvent.DeltaGold < 0) {
		m_node.Call("lose_gold_effect");
	}
	else if (triggeredEvent.DeltaGold > 0) {
		m_node.Call("gain_gold_effect");
	}
	if (Gold < 0) {
		Gold = 0;
	}

	Health += triggeredEvent.DeltaHealth;
	if (triggeredEvent.DeltaHealth < 0) {
		m_node.Call("lose_health_effect");
	}
	else if (triggeredEvent.DeltaHealth > 0) {
		m_node.Call("gain_health_effect");
	}
	if (Health > 100) {
		Health = 100;
	}

	CheckEndCondition();
	
	m_node.Call("draw_event", triggeredEvent.Title, triggeredEvent.Description, triggeredEvent.ChoiceDescriptions, "");

	m_state = State.WAIT_CHOICE_EVENT;
	m_eventId = triggered;

	return;
}

// HandleEventChoice(int)
// Chooses an available option for responding to an event (possibly firing another event in the process).
// Invalid events will be ignored.
// =============================================
// choice:    User's choice.
private void HandleEventChoice(int choice) {
	// Invalid choice
	if (choice < 0 || choice >= m_events[m_eventId].NumChoices()) {
		return;
	}

	Event currentEvent = m_events[m_eventId];

	float choiceSuccess = currentEvent.ChoiceSuccessChance[choice];

	int dest = currentEvent.ChoiceSuccess[choice];

	if (choiceSuccess != 1.0) {
		float randomNumber = (float) m_random.NextDouble();
		
		if (randomNumber > choiceSuccess) {
			dest = currentEvent.ChoiceFailure[choice];
		}
	}
	
	// Return to map
	if (dest == -1) {
		m_state = State.WAIT_CHOICE_NODE;

		Food += currentEvent.DeltaFood;
		if (currentEvent.DeltaFood < 0) {
			m_node.Call("lose_food_effect");
		}
		else if (currentEvent.DeltaFood > 0) {
			m_node.Call("gain_food_effect");
		}
		if (Food < 0) {
			Food = 0;
		}

		Gold += currentEvent.DeltaGold;
		if (currentEvent.DeltaGold < 0) {
			m_node.Call("lose_gold_effect");
		}
		else if (currentEvent.DeltaGold > 0) {
			m_node.Call("gain_gold_effect");
		}
		if (Gold < 0) {
			Gold = 0;
		}
		
		Health += currentEvent.DeltaHealth;
		if (currentEvent.DeltaHealth < 0) {
			m_node.Call("lose_health_effect");
		}
		else if (currentEvent.DeltaHealth > 0) {
			m_node.Call("gain_health_effect");
		}
		if (Health > 100) {
			Health = 100;
		}

		PrintMap();
		
		if (Food <= 0 && !m_hasStarved) {
			GD.Print("Starve: Lose health");
			Health -= 10;
			
			m_node.Call("lose_health_effect");
			
			// starving event
			m_state = State.WAIT_CHOICE_EVENT;
			m_eventId = 14;
			m_node.Call("draw_event", 
						m_events[m_eventId].Title, 
						m_events[m_eventId].Description, 
						m_events[m_eventId].ChoiceDescriptions,
						m_events[m_eventId].Ascii);

			m_hasStarved = true;
		} else {
			m_hasStarved = false;
		}
		
		CheckEndCondition();

	} else {

		Food += currentEvent.DeltaFood;
		if (currentEvent.DeltaFood < 0) {
			m_node.Call("lose_food_effect");
		}
		else if (currentEvent.DeltaFood > 0) {
			m_node.Call("gain_food_effect");
		}
		if (Food < 0) {
			Food = 0;
		}

		Gold += currentEvent.DeltaGold;
		if (currentEvent.DeltaGold < 0) {
			m_node.Call("lose_gold_effect");
		}
		else if (currentEvent.DeltaGold > 0) {
			m_node.Call("gain_gold_effect");
		}
		if (Gold < 0) {
			Gold = 0;
		}
		
		Health += currentEvent.DeltaHealth;
		if (currentEvent.DeltaHealth < 0) {
			m_node.Call("lose_health_effect");
		}
		else if (currentEvent.DeltaHealth > 0) {
			m_node.Call("gain_health_effect");
		}
		if (Health > 100) {
			Health = 100;
		}

		GD.Print("Deltas: ", currentEvent.DeltaHealth, " ", currentEvent.DeltaGold, " ", currentEvent.DeltaFood);
		GD.Print("Health: ", Health, ", Gold: ", Gold, ", Food: ", Food);
		
		CheckEndCondition();

		Event nextEvent = m_events[dest];
		
		m_node.Call("draw_event", 
					nextEvent.Title, 
					nextEvent.Description, 
					nextEvent.ChoiceDescriptions,
					nextEvent.Ascii);

		m_state = State.WAIT_CHOICE_EVENT;
		m_eventId = dest;
	}
}


// PrintMap(void)
// Constructs and draws a view of the map based on the current node the user is present in, marking the node with *.
// =============================================
// No arguments.
private void PrintMap() {	
	SeaNode currentNode = m_nodes[m_nodeId];

	int centerRow = currentNode.Row;
	int centerCol = currentNode.Col;
	
	int top = centerRow - (gridHeight - 1) / 2;
	int bottom = centerRow + (gridHeight - 1) / 2;
	if (top < 0) {
		top = 0;
		bottom = gridHeight - 1;
	}
	else if (bottom >= mapHeight) {
		top = mapHeight - gridHeight;
		bottom = mapHeight - 1;
	}

	int left = centerCol - (gridWidth - 1) / 2;
	int right = centerCol + (gridWidth - 1) / 2;
	if (left < 0) {
		left = 0;
		right = gridWidth - 1;
	}
	else if (right >= mapWidth) {
		left = mapWidth - gridWidth;
		right = mapWidth - 1;
	}
	
	// Construct the map as a string
	StringBuilder sb = new StringBuilder();
	char prev = m_map[currentNode.Row, currentNode.Col];
	m_map[currentNode.Row, currentNode.Col] = '*';

	for (int row = top; row <= bottom; ++row) {
		for (int col = left; col <= right; ++col) {
			sb.Append(m_map[row, col]);
		}
		sb.Append("\n");
	}
	
	m_map[currentNode.Row, currentNode.Col] = prev;
	
	int[] adjList = currentNode.AdjList;	
	m_node.Call("draw_map", sb.ToString(), (object) adjList, top, left);

	// DrawMap(sb.ToString(), adjList, numRows, numCols);
}


public int[] NodeCoordinates(int nodeIndex) {
	int[] ret = new int[2];
	ret[0] = m_nodes[nodeIndex].Row;
	ret[1] = m_nodes[nodeIndex].Col;
	return ret;
}


public string NodeName(int nodeIndex) {
	return m_nodes[nodeIndex].Name;
}


private void CheckEndCondition() {
	if (Health <= 0) {
		m_eventId = 13;
		Event damageDeath = m_events[m_eventId];

		m_state = State.WAIT_CHOICE_EVENT;
		m_node.Call("draw_event", 
					damageDeath.Title, 
					damageDeath.Description, 
					damageDeath.ChoiceDescriptions,
					damageDeath.Ascii);
		// exit
	}
}


// StartGame(void)
// =============================================
// Start the game by calling the start event (22), and asking for it to be rendered.
void StartGame() {
	Event startEvent = m_events[m_eventId];

	m_node.Call("draw_event",
				startEvent.Title,
				startEvent.Description,
				startEvent.ChoiceDescriptions,
				startEvent.Ascii);
}


// GAME RESOURCES 
private char[,] m_map = null;
private SeaNode[] m_nodes = null;

private State m_state;
private Event[] m_events = null;
private int[] m_validRandomEvents = null;
private float[] m_eventWeights = null;
private float m_totalWeight = 0.0f;
private Random m_random = null;

// USER RESOURCES
private int m_nodeId = 13;
private int m_eventId = 22;

public int Health { get; set; } = 100;
public int Gold { get; set; } = 100;
public int Food { get; set; } = 100;

private bool m_hasStarved = false;

private Godot.Object m_node;
}
