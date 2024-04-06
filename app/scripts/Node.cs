class Node 
{
	// Node(NodeId, EventId)
	// Instantiates Node with a NodeId, and the EventId for the node.
	// If uEvent is >0, the node does NOT contain an event.
	public Node(NodeId id, EventId uEvent) {
		m_id = id;
		m_uEvent = uEvent;
	}
	
	public ~Node();

	// Returns the node-specific event for this node.
	public EventId GetEvent() {
		return m_uEvent;
	}

	private NodeId m_id;		// Node's UUID
	private EventId m_uEvent; 	// Node-specific event's UUID
};



