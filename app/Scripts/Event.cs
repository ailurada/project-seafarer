// Event.cs
//
// Implements the Event dataclass.

using Godot;

public class Event
{
	public Event(int id, string title, string description, float probability, string[] choiceDescriptions, int[] choiceDestinations, int deltaFood, int deltaGold, int deltaHealth, string ascii) {
		ID = id;
		Title = title;
		Description = description;
		Probability = probability;
		ChoiceDescriptions = choiceDescriptions;
		ChoiceDestinations = choiceDestinations;
		DeltaFood = deltaFood;
		DeltaGold = deltaGold;
		DeltaHealth = deltaHealth;
		Ascii = ascii;
	}
	
	public Event() {
		ID = -1;
		Probability = 0;
		Title = "";
		Description = "";
		ChoiceDescriptions = null;
		ChoiceDestinations = null;
		
		DeltaFood = 0;
		DeltaGold = 0;
		DeltaHealth = 0;
		Ascii = null;
	}
		
	// For debug purposes only
	public void PrintEvent() {
		GD.Print("ID: ", ID, ", Probability: ", Probability.ToString(), ", Title: ", Title, ", Description: ", Description);

		for (int i = 0; i < ChoiceDescriptions.Length; ++i) {
			GD.Print(ChoiceDescriptions[i], ": ", ChoiceDestinations[i]);
		}

	}
	
	public int ID { get; }
	public float Probability { get; }
	public string Title { get; }
	public string Description { get; }
	public string[] ChoiceDescriptions { get; }
	public int[] ChoiceDestinations { get; }
	public int DeltaFood { get; }
	public int DeltaGold { get; }
	public int DeltaHealth { get; }
	public string Ascii { get; }
	public int NumChoices() { return ChoiceDescriptions.Length; }
}
