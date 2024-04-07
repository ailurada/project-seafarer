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
		m_state = State.WAIT_CHOICE_NODE;
		
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
	m_events = json_node.LoadEvents("res://data/events.json", out int num_events);
	m_nodes = json_node.LoadSeaNodes("res://data/nodes.json", out int num_nodes);
	
	// Load event weights
	m_eventWeights = new float[num_events];
	m_totalWeight = 0.0f;
	for (int i = 0; i < num_events; ++i) {
		m_eventWeights[i] = m_totalWeight;
		m_totalWeight += m_events[i].Probability;
	}
	
	// Seed randomization
	m_random = new Random();

	// ========== End Loading ==========  
	
	// Start the game!
	StartGame();

	PrintMap();
	
	}
	catch(Exception e) {GD.Print(e);}
}

int RandomEvent() {
	float randomNumber = (float) m_random.NextDouble() * m_totalWeight;
	int eventIndex = 0;
	while (eventIndex < m_nodes.Length - 1 && m_eventWeights[eventIndex + 1] < randomNumber) {
		++eventIndex;
	}
	return eventIndex;
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
		Gold -= 10;
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
	Food -= 10;
	
	m_nodes[destination].Visit();
	m_map[m_nodes[destination].Row, m_nodes[destination].Col] = '@';

	int triggered = RandomEvent();
	Event triggeredEvent = m_events[triggered];

	Food += triggeredEvent.DeltaFood;
	Gold += triggeredEvent.DeltaGold;
	Health += triggeredEvent.DeltaHealth;
	CheckEndCondition();
	
	m_node.Call("draw_event", triggeredEvent.Title, triggeredEvent.Description, triggeredEvent.ChoiceDescriptions);

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
	// If the event is no choice
	if (m_eventId == -1) {
		m_state = State.WAIT_CHOICE_NODE;
		PrintMap();
		
		if (Food <= 0) {
			Health -= 10;
			// starving event
			m_node.Call("draw_event", m_events[14].Title, m_events[14].Description, m_events[14].ChoiceDescriptions);

		}
		
		CheckEndCondition();
		
		return;		
	}

	// Invalid choice
	if (choice < 0 || choice > m_events[m_eventId].NumChoices()) {
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
		PrintMap();
		
		if (Food <= 0) {
			Health -= 10;
			// starving event
			m_node.Call("draw_event", m_events[14].Title, m_events[14].Description, m_events[14].ChoiceDescriptions);

		}
		
		CheckEndCondition();
	}
	else {

		Food += currentEvent.DeltaFood;
		Gold += currentEvent.DeltaGold;
		Health += currentEvent.DeltaHealth;
		CheckEndCondition();
		
		m_node.Call("draw_event", 
					currentEvent.Title, 
					currentEvent.Description, 
					currentEvent.ChoiceDescriptions,
					currentEvent.Ascii);

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
		m_node.Call("draw_event", 
					"Death!", 
					"Due to continuous and multiple injuries suffered by your body without proper care, your body has stopped cooperating with you.", 
					new string[]{ "Okay." },
					"null");
		// exit
	}
	if (Food <= 0) {
		m_node.Call("draw_event", 
					"Death!", 
					"Long voyages with no resupply has left the ship with no food or any edible object to speak of. You and your crew suffer a slow, painful death by starvation.", 
					new string[]{ "Okay." },
					"null");
		//DrawEvent("Death!", "Long voyages with no resupply has left the ship with no food or any edible object to speak of. You and your crew suffer a slow, painful death by starvation.", { "Okay." });
		// exit
	}
}


// StartGame(void)
// =============================================
// Start the game by calling the start event (22), and asking for it to be rendered.
void StartGame() {
	Event startEvent = m_events[22];

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
private float[] m_eventWeights = null;
private float m_totalWeight = 0.0f;
private Random m_random = null;

// USER RESOURCES
private int m_nodeId = 21;
private int m_eventId = -1;

public int Health { get; set; } = 100;
public int Gold { get; set; } = 100;
public int Food { get; set; } = 100;

private Godot.Object m_node;
}
