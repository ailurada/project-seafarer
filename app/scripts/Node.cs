// Node.cs
//
// Implements the node class, the representation of the sea nodes on the map.

class Node 
{
	// Node()
	// Instantiates Node with default values.
	// ============================================= 
	// No parameters.
	public Node() {
		m_id = -1;
		m_uEvent = -1;
		
		m_visited = false;
		m_name = "";
		m_description = "";
	}
	
	// Node(NodeId, EventId, bool, string, string)
	// Instantiates node with the given values.
	// =============================================
	// NodeId: 	       The unique id of this sea node
	// EventId:	       The id of the event unique to this node
	// visited:	       The status of the node (has this node been visited yet?)
	// name:	       The node's title/name
	// description:    The flavor text description of the node
	public Node(NodeId id, EventId eventId, bool visited, string name, string description) {
		m_id = id;
		m_uEvent = eventId;
		m_visited = visited;
		m_name = name;
		m_description = description;

	}
	
	public ~Node();

	// GetEvent()
	// Returns the node specific event's ID.
	// =============================================
	// No parameters.
	public EventId GetEvent() {
		return m_uEvent;
	}

	private NodeId m_id;		    // Node's UUID
	private EventId m_uEvent; 	    // Node-specific event's UUID

	private bool m_visited;		    // Node's visitation status
	private string m_name;		    // Node's name
	private string m_description;   // Node's description
};



