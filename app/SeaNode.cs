// SeaNode.cs
//
// Implements the node class, the representation of the sea nodes on the map.

using Godot;

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
    // eventId:	       The id of the event unique to this node
    // name:	       The node's title/name
    // description:    The flavor text description of the node
    // visited:	       The status of the node (has this node been visited yet?)
    public SeaNode(int ID, int eid, string title, string desc, int[] adjacent, bool seen) {
        id = ID;
        eventID = eid;
        name = title;
        description = desc;
        adjList = adjacent;
        visited = seen;
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

    private int id;
    private int eventID;
    private string name;
    private string description;
    private int[] adjList;
    private bool visited;
};