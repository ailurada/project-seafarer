
class Event
{
	public Event(EventId id, float probability, string title, string m_description, Choice[] choices) {
		m_id = id;
		m_probability = probability;
		m_title = title;
		m_description = description;
		m_choices = choices;
	}
	
	public Event() {
		m_id = -1;
		m_probability = 0;
		m_title = "";
		m_description = "";
		m_choices = null;
	}
	
	public struct Choice {
		public string m_description;
		public EventId m_destination;
	}
	
	public string GetTitle() { return m_title; }
	public string GetDescription() { return m_description; }
	public float GetDescription() { return m_probability; }
	public Choice[] GetChoices() { return m_choices; }
	
	private EventId m_id;
	private float m_probability;
	private string m_title;
	private string m_description;
	private Choice[] m_choices;
}
