class Node 
{
	public Node(NodeId id, EventId uEvent) {
		m_id = id;
		m_uEvent = uEvent;
	}
	
	public ~Node();

	public EventId GetEvent() {
		return m_uEvent;
	}

	private NodeId m_id;
	private EventId m_uEvent; 	
};



