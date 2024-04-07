// SeaNode.cs
//
// Implements the node class, the representation of the sea nodes on the map.

using Godot;
using System;

public class SeaNode {

	public SeaNode() {
		id = -1;
		eventID = -1;
		name = "null";
		description = "null";
		visited = false;
	}

	// SeaNode(int, int, string, string, bool)
	// Instantiates node with the given values.
	// =============================================
	// id: 	       	   The unique id of this sea node
	// eid:	           The id of the event unique to this node
	// coords:         The coordinate csv pair
	// name:	       The node's title/name
	// description:    The flavor text description of the node
	// visited:	       The status of the node (has this node been visited yet?)
	public SeaNode(int ID, int eid, string coords, string title, string desc, int[] adjacent, bool seen) {
		id = ID;
		eventID = eid;
		name = title;
		description = desc;
		adjList = adjacent;
		visited = seen;

		// Split the input coord string into two, parse and store
		string[] coordStrings = coords.Split(',');
		_row = Int32.Parse(coordStrings[0]);
		_col = Int32.Parse(coordStrings[1]);
	}

	~SeaNode() {
		// No body.
	}

	// For debug purposes only.
	public void printNode() {
		GD.Print("ID: ", id, ", EID: ", eventID, ", Name: ", name, ", Description: ", description, ", visited: ", visited);
		GD.Print("ADJACENCY LIST: ");
		printAdjNodes();
	}

	public void printAdjNodes() {
		for (int i = 0; adjList[i] != -1; ++i) {
			GD.Print(adjList[i]);
		}
	}
	
	public string GetName() {
		return name;
	}

	public string GetDescription() {
		return description;
	}
		
	public int[] GetAdjacencyList() {
		return adjList;
	}
	
	public void Visit() {
		visited = true;
	}


	// Get the position of the current SeaNode
	private int id;
	private int eventID;
	private int _row;
	private int _col;
	private string name;
	private string description;
	private int[] adjList;
	private bool visited;
	
	public int Row {
		get { return _row; }
		set { _row = value; }
	}
	public int Col {
		get { return _col; }
		set { _col = value; }
	}
};
