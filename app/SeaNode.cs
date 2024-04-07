// SeaNode.cs
//
// Implements the node class, the representation of the sea nodes on the map.

using Godot;
using System;

public class SeaNode {

	public SeaNode() {
		ID = -1;
		EventID = -1;
		Name = "null";
		Description = "null";
		Visited = false;
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
	public SeaNode(int id, int eid, string coords, string name, string description, int[] adjacent, bool visited) {
		ID = ID;
		EventID = eid;
		Name = name;
		Description = description;
		AdjList = adjacent;
		Visited = visited;

		// Split the input coord string into two, parse and store
		string[] coordStrings = coords.Split(',');
		Row = int.Parse(coordStrings[0]);
		Col = int.Parse(coordStrings[1]);
	}

	~SeaNode() {
		// No body.
	}

	// For debug purposes only.
	public void PrintNode() {
		GD.Print("ID: ", ID, ", EID: ", EventID, ", Name: ", Name, ", Description: ", Description, ", visited: ", Visited);
		GD.Print("ADJACENCY LIST: ");
		PrintAdjNodes();
	}

	public void PrintAdjNodes() {
		for (int i = 0; AdjList[i] != -1; ++i) {
			GD.Print(AdjList[i]);
		}
	}
	
	public void Visit() {
		Visited = true;
	}


	public int ID { get; }
	public int EventID { get; }
	public int Row { get; }
	public int Col { get; }
	public string Name { get; }
	public string Description { get; }
	public int[] AdjList { get; }
	public bool Visited { get; set; }
};
