// Event.cs
//
// Implements the Event dataclass.

using Godot;

class Event
{
	public Event(int id, string title, string description, float probability, string[] choiceDescriptions, int[] choiceDestinations, int deltaFood, int deltaGold, int deltaHealth) {
		m_id = id;
		m_title = title;
		m_description = description;
		m_probability = probability;
		m_choiceDescriptions = choiceDescriptions;
		m_choiceDestinations = choiceDestinations;
		m_deltaFood = deltaFood;
		m_deltaGold = deltaGold;
		m_deltaHealth = deltaHealth;
	}
	
	public Event() {
		m_id = -1;
		m_probability = 0;
		m_title = "";
		m_description = "";
		m_choiceDescriptions = null;
		m_choiceDestinations = null;
		
		m_deltaFood = 0;
		m_deltaGold = 0;
		m_deltaHealth = 0;
	}
		
	public string GetTitle() { return m_title; }
	public string GetDescription() { return m_description; }
	public int NumChoices() { return m_choiceDescriptions.Length; }
	public string[] GetChoiceDescriptions() { return m_choiceDescriptions; }
	public int[] GetChoiceDestinations() { return m_choiceDestinations; }
	public float GetProbability() { return m_probability; }

	public int GetDeltaFood() { return m_deltaFood; }
	public int GetDeltaGold() { return m_deltaGold; }
	public int GetDeltaHealth() { return m_deltaHealth; }

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
	
	private int m_deltaFood;
	private int m_deltaGold;
	private int m_deltaHealth;
}
