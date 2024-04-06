class Node 
{
	// Node(NodeId, EventId)
	// Instantiates Node with a NodeId, and the EventId for the node.
	// If uEvent is >0, the node does NOT contain an event.

	public Node() {
		m_id = -1;
		m_uEvent = -1;
		
		m_visited = false;
		m_name = "";
		m_description = "";
	}
	
	public ~Node();

	// Returns the node-specific event for this node.
	public EventId GetEvent() {
		return m_uEvent;
	}

	public Initialize(NodeId id, EventId eventId, bool visited, string name, string description) {
		m_id = id;
		m_uEvent = eventId;
		m_visited = visited;
		m_name = name;
		m_description = description;
	}

	private NodeId m_id;		// Node's UUID
	private EventId m_uEvent; 	// Node-specific event's UUID

	private bool m_visited;
	private string m_name;
	private string m_description;
};



