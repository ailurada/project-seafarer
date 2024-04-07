// Event.cs
//
// Implements the Event dataclass.

using Godot;

class Event
{
	public Event(int id, string title, string description, float probability, string[] choiceDescriptions, int[] choiceDestinations) {
		m_id = id;
		m_title = title;
		m_description = description;
		m_probability = probability;
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
	public int[] GetChoiceDestinations() { return m_choiceDestinations; }

	// For debug purposes only
	public void printEvent() {
		GD.Print("ID: ", m_id, ", Probability: ", m_probability.ToString(), ", Title: ", m_title, ", Description: ", m_description);

		for (int i = 0; i < m_choiceDescriptions.Length; ++i) {
			GD.Print(m_choiceDescriptions[i], ": ", m_choiceDestinations[i]);
		}

	}
	
	private int m_id;
	private float m_probability;
	private string m_title;
	private string m_description;
	private string[] m_choiceDescriptions;
	private int[] m_choiceDestinations;
}
