
class Event
{
	public Event(EventId id, float probability, string title, string m_description, string[] choiceDescriptions, EventId[] m_choiceDestinations) {
		m_id = id;
		m_probability = probability;
		m_title = title;
		m_description = description;
		m_choiceDescriptions = choiceDescriptions;
		m_choiceDestinations = choiceDestinations;
	}
	
	public Event() {
		m_id = -1;
		m_probability = 0;
		m_title = "";
		m_description = "";
		m_choiceDescriptions = null;
		m_choiceDestinations = null;
	}
		
	public string GetTitle() { return m_title; }
	public string GetDescription() { return m_description; }
	public int NumChoices() { return m_choiceDescriptions.Length; }
	public string[] GetChoiceDescriptions() { return m_choiceDescriptions; }
	public EventId[] GetChoiceDestinations() { return m_choiceDestinations; }
	
	private EventId m_id;
	private float m_probability;
	private string m_title;
	private string m_description;
	private string[] m_choiceDescriptions;
	private EventId[] m_choiceDestinations;
}
