class Node 
{
	public Node() {
		m_id = -1;
		m_uEvent = -1;
		
		m_visited = false;
		m_name = "";
		m_description = "";
	}
	
	public ~Node();

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


	private NodeId m_id;
	private EventId m_uEvent;
	
	private bool m_visited;
	private string m_name;
	private string m_description;
};



