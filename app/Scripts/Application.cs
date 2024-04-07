using System;
using System.Text;
using System.IO;

// should know grid size, somehow...
const int gridHeight = 15;
const int gridWidth = 41;
// should know map size, somehow, too....
const int mapHeight = 140;
const int mapWidth = 400;


public enum State {
	WAIT_CHOICE_NODE,
	WAIT_CHOICE_EVENT
}

class Application
{
public Application() {}
public ~Application() {}

public Initialize() {
	// call files...
	// Init Nodes
	// Init Events
	// read the map!
	
	// move reading part to somewhere else?
	string filePath = "path_to_map_file.txt";
	string fileContents = File.ReadAllText(filePath);
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

public void UserInput(int input) {
	if (m_state == WAIT_CHOICE_NODE) {
		NodeChoiceHandler(input - 1);
	}
	else if (m_state == WAIT_CHOICE_EVENT) {
		EventChoiceHandler(input - 1);
	}
}

private void NodeChoiceHandler(int choice) {
	if (choice < 0 || choice > m_adjacencyList.GetLength(1)) {
		return;
	}

	if (choice < m_adjacencyList.GetLength(1)) {
		TravelEdge(m_adjacencyList[m_nodeId, choice]);
	}
}

private void TravelEdge(int destination) {
	m_nodeId = destination;

	// using System // needed for random
	Random random = new Random(); // move to init
	int randomNumber = random.Next(0, 19);

	// mapping data structure (array) during event init
	// EventId fired = ...;

	
	// DrawEvent(m_events[fired].GetTitle(), m_events[fired].GetDescription(), m_events[fired].GetChoiceDescriptions());
	m_state = WAIT_CHOICE_EVENT;
	m_eventId = fired;
	return;
}

private EventChoiceHandler(int choice) {
	if (choice < 0 || choice > m_events[m_eventId].NumChoices()) {
		return;
	}

	EventId dest = (m_events[m_eventId].GetChoiceDestinations())[choice];

	if (dest == -1) {
		m_state = WAIT_CHOICE_NODE;
		return;
	}
	else {
		// DrawEvent(m_events[fired].GetTitle(), m_events[fired].GetDescription(), m_events[fired].GetChoiceDescriptions());
		m_state = WAIT_CHOICE_EVENT;
		m_eventId = dest;
	}
}

private PrintMap() {	
	int centerRow = m_nodes[m_nodeId].GetRow();
	int centerCol = m_nodes[m_nodeId].GetCol();
	
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
	
	// construct the map as a string
	StringBuilder sb = new StringBuilder();
	char prev = m_map[m_nodes[m_nodeId].GetRow(), m_nodes[m_nodeId].GetCol()];
	m_map[m_nodes[m_nodeId].GetRow(), m_nodes[m_nodeId].GetCol()] = '*';
	for (int row = top; row < bottom; ++row) {
		for (int col = left; col < right; ++col) {
			sb.Append(m_map[row, col]);
		}
		sb.Append("\n");
	}
	m_map[m_nodes[m_nodeId].GetRow(), m_nodes[m_nodeId].GetCol()] = prev;
	
	// DrawMap(sb.ToString());
}


private char[,] m_map = null; // INIT BY READING MAP FILE

private State m_state = WAIT_CHOICE_NODE;
private SeaNode[] m_nodes = null;
private NodeId[,] m_adjacencyList = null;
private Event[] m_events = null;

// USER STATE
private NodeId m_nodeId;
private EventId m_eventId;

private int m_health;
private int m_gold;


}

