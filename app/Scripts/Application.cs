// Application.cs
//
// Implements the Application class, the central manager and authority on game logic.

using System;
using System.Text;
using System.IO;


// These four values should be included in Types.cs or similar.
const int gridHeight = 15;
const int gridWidth = 41;
const int mapHeight = 140;
const int mapWidth = 400;


// The state in which the game (should be) at.
// Both are waiting for user input; the difference is what the input should address.
public enum State {
	WAIT_CHOICE_NODE,
	WAIT_CHOICE_EVENT
}


class Application
{
// No body for neither the constructor nor the destructor
public Application() {}
public ~Application() {}


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
public void Initialize() {
	// call files...
	// Init Nodes
	// Init Events
	// read the map!

	string fileContents = readFileMap("res://data/map.txt")
	
	int index = 0;
	m_map = new char[mapHeight, mapWidth];
	for (int row = 0; row < mapHeight; ++row) {
		for (int col = 0; col < mapWidth; ++col) {
			m_map[row, col] = fileContents[index];
			++index;
		}
		++index;
	}
}


// UserInput(int)
// Handles the given user input, received as an integer input.
// =============================================
// input:	User's input.
public void UserInput(int input) {
	if (m_state == WAIT_CHOICE_NODE) {
		NodeChoiceHandler(input - 1);
	}
	else if (m_state == WAIT_CHOICE_EVENT) {
		EventChoiceHandler(input - 1);
	}
}


// NodeChoiceHandler(int)
// Handles the given user choice, received as an integer input.
// Invalid choices will be ignored.
// =============================================
// choice:	User's choice.
private void NodeChoiceHandler(int choice) {
	// TODO: add option for resting, and other additional options that can be expected from a sea node.
	if (choice < 0) {
		return;
	}

	int[] adjList = m_nodes[m_nodeId].GetAdjacencyList(); 
	if (choice < adjList.Length) {
		TravelEdge(adjList[choice]);
	}
	// option after travel is rest
	else if (choice == adjList.Length) {
		m_health += 10;
		if (m_health > 100) {
			m_health = 100;
		}
	}
	// option after rest is resupply
	else if (choice > adjList.Length) {
		if (m_gold < 10) {
			//DrawEvent("You're poor!", "You do not have enough money for this.", { "Okay." });
			m_state = WAIT_CHOICE_EVENT;
			m_eventId = -1; // signifies that we're just waiting for a response. any response.
			return;
		}
		m_food += 10;
		m_gold -= 10;
		if (m_food > 100) {
			m_food = 100;
		}
	}
}


// TravelEdge(int)
// Travels to the desired SeaNode.
// A random event will be fired.
// =============================================
// choice:    User's choice.
private void TravelEdge(int destination) {
	m_nodeId = destination;
	m_food -= 10;
	
	m_nodes[destination].Visit();
	m_map[m_nodes[destination].Row, m_nodes[destination].Col] = '@';

	// using System // needed for random
	Random random = new Random(); // move to init
	int randomNumber = random.Next(0, 19);

	// mapping data structure (array) during event init
	// EventId fired = ...;

	
	m_food += m_events[fired].GetDeltaFood();
	m_gold += m_events[fired].GetDeltaGold();
	m_health += m_events[fired].GetDeltaHealth();
	GameOver();
	
	// DrawEvent(m_events[fired].GetTitle(), m_events[fired].GetDescription(), m_events[fired].GetChoiceDescriptions());
	m_state = WAIT_CHOICE_EVENT;
	m_eventId = fired;


	return;
}

// EventChoiceHandler(int)
// Chooses an available option for responding to an event (possibly firing another event in the process).
// Invalid events will be ignored.
// =============================================
// choice:    User's choice.
private EventChoiceHandler(int choice) {
	if (m_eventId == -1) {
		m_state = WAIT_CHOICE_NODE;
		PrintMap();
		return;		
	}
	if (choice < 0 || choice > m_events[m_eventId].NumChoices()) {
		return;
	}

	int dest = (m_events[m_eventId].GetChoiceDestinations())[choice];

	if (dest == -1) {
		m_state = WAIT_CHOICE_NODE;
		PrintMap();
	}
	else {
		m_food += m_events[m_eventId].GetDeltaFood();
		m_gold += m_events[m_eventId].GetDeltaGold();
		m_health += m_events[m_eventId].GetDeltaHealth();
		GameOver();
		
		// DrawEvent(m_events[fired].GetTitle(), m_events[fired].GetDescription(), m_events[fired].GetChoiceDescriptions());
		m_state = WAIT_CHOICE_EVENT;
		m_eventId = dest;
	}
}


// PrintMap(void)
// Constructs and draws a view of the map based on the current node the user is present in, marking the node with *.
private void PrintMap() {	
	int centerRow = m_nodes[m_nodeId].Row;
	int centerCol = m_nodes[m_nodeId].Col;
	
	int top = centerRow - (gridHeight - 1) / 2;
	int bottom = centerRow + (gridHeight - 1) / 2;
	if (top < 0) {
		top = 0;
		bottom = gridHeight - 1;
	}
	else if (bottom >= mapHeight) {
		top = mapHeight - gridHeight - 2;
		bottom = mapHeight - 1;
	}

	int left = centerCol - (gridWidth - 1) / 2;
	int right = centerCol + (gridWidth - 1) / 2;
	if (left < 0) {
		left = 0;
		right = gridWidth - 1;
	}
	else if (right >= mapWidth) {
		left = mapWidth - gridWidth - 2;
		right = gridWidth - 1;
	}
	
	// Construct the map as a string
	StringBuilder sb = new StringBuilder();
	char prev = m_map[m_nodes[m_nodeId].Row, m_nodes[m_nodeId].Col];
	m_map[m_nodes[m_nodeId].Row, m_nodes[m_nodeId].Col] = '*';
	for (int row = top; row < bottom; ++row) {
		for (int col = left; col < right; ++col) {
			sb.Append(m_map[row, col]);
		}
		sb.Append("\n");
	}
	m_map[m_nodes[m_nodeId].Row, m_nodes[m_nodeId].Col] = prev;
	
	int[] adjList = m_nodes[m_nodeId].GetAdjacencyList();
	int[,] adjListCoords = new int[adjList.Length, 2];
	for (int i = 0; i < adjList.Length; ++i) {
		adjListCoords[i, 0] = m_nodes[m_nodeId].Row;
		adjListCoords[i, 1] = m_nodes[m_nodeId].Col;
	}
	
	// DrawMap(sb.ToString(), adjListCoords, numRows, numCols);
}

private void GameOver() {
	if (m_health <= 0) {
		//DrawEvent("Death!", "Due to continuous and multiple injuries suffered by your body without proper care, your body has stopped cooperating with you.", { "Okay." });
		// exit
	}
	if (m_food <= 0) {
		//DrawEvent("Death!", "Long voyages with no resupply has left the ship with no food or any edible object to speak of. You and your crew suffer a slow, painful death by starvation.", { "Okay." });
		// exit
	}
}

// GAME RESOURCES 
private char[,] m_map = null;
private SeaNode[] m_nodes = null;

private State m_state = WAIT_CHOICE_NODE;
private Event[] m_events = null;


// USER RESOURCES
private int m_nodeId = -1;
private int m_eventId = -1;

private int m_health = 100;
private int m_gold = 100;
private int m_food = 100;
}
