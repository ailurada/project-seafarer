// json.cs
//
// Implements JSON functions and the ability to 
// load SeaNodes and events from files.

using Godot;

public class Json : Node
{
	// Given a filePath, read the JSON from the file and return the 
	// JSON as a dictionary.
	Godot.Collections.Dictionary ReadJSON(string filePath) {
		File file = new File();
		file.Open(filePath, File.ModeFlags.Read);
		string rawJSON = file.GetAsText();
		file.Close();

		JSONParseResult jsonResult = JSON.Parse(rawJSON);
		var dict = jsonResult.Result as Godot.Collections.Dictionary;
		
		return dict;
	}

		
	// Given a filePath, read all the JSON from the file and translate it
	// into a list of nodes.
	public SeaNode[] LoadSeaNodes(string filePath, out int amount) {
		Godot.Collections.Dictionary dict = ReadJSON(filePath);
		int amtNodes = dict.Count;
		amount = amtNodes;
		SeaNode[] nodeList = new SeaNode[amtNodes];

		// For each node in the JSON file, translate to a SeaNode object and store in the array.
		for (int i = 0; i < amtNodes; ++i) {
			Godot.Collections.Dictionary nodeInfo = dict[i.ToString()] as Godot.Collections.Dictionary;

			// Translate each adjacency list into digestible form
			var adjArray = nodeInfo["adjacent"] as Godot.Collections.Array;

			int adjCount = adjArray.Count;
			int j = 0;

			int[] adjList = new int[adjCount];

			for (; j < adjCount; ++j)
				adjList[j] = int.Parse((string)adjArray[j]);

			SeaNode newNode = new SeaNode(
				i,
				int.Parse((string)nodeInfo["eid"]),
				(string)nodeInfo["coords"],
				(string)nodeInfo["title"],
				(string)nodeInfo["description"],
				adjList,
				(bool)nodeInfo["visited"]
			);

			nodeList[i] = newNode;
		}

		return nodeList;
	}

	// Given a filePath, read all the JSON from the file and translate it
	// into a list of events. Returns total number of events, and total number of WEIGHTED events.
	public Event[] LoadEvents(string filePath, out int amount, out int weighted) {
		Godot.Collections.Dictionary dict = ReadJSON(filePath);
		int amtEvents = dict.Count;
		amount = amtEvents;
		weighted = 0;

		Event[] eventList = new Event[amtEvents];

		// For each event in the JSON file, translate to an Event object and store in the array.
		for (int i = 0; i < amtEvents; ++i) {
			var eventInfo = dict[i.ToString()] as Godot.Collections.Dictionary;

			// Translate choice descriptions to a string array
			var descArr = eventInfo["choice_descriptions"] as Godot.Collections.Array;

			int numDescs = descArr.Count;
			string[] descList = new string[numDescs];

			int j = 0;
			for (; j < numDescs; ++j) {
				descList[j] = (string)descArr[j];
			}

			// Translate choice success __chances__ to a float array
			var chanceArr = eventInfo["choice_success_chance"] as Godot.Collections.Array;

			int numChances = chanceArr.Count;
			float[] chanceList = new float[numChances];

			j = 0;
			for(; j < numChances; ++j) {
				chanceList[j] = float.Parse((string)chanceArr[j]);
			}

			// Translate choice successes to a int array
			var successArr = eventInfo["choice_success"] as Godot.Collections.Array;

			int numSuccesses = successArr.Count;
			int[] successList = new int[numSuccesses];

			j = 0;
			for (; j < numSuccesses; ++j) {
				successList[j] = int.Parse((string)successArr[j]);
			}

			// Translate choice failures to an int array
			var failureArr = eventInfo["choice_failure"] as Godot.Collections.Array;

			int numFailures = failureArr.Count;
			int[] failureList = new int[numFailures];

			j = 0;
			for (; j < numFailures; ++j) {
				failureList[j] = int.Parse((string)failureArr[j]);
			}
			Event newEvent = new Event(
				i,
				(string)eventInfo["title"],
				(string)eventInfo["description"],
				float.Parse((string)eventInfo["probability"]),
				descList,
				chanceList,
				successList,
				failureList,
				int.Parse((string)eventInfo["delta_food"]),
				int.Parse((string)eventInfo["delta_gold"]),
				int.Parse((string)eventInfo["delta_health"]),
				(string)eventInfo["ascii"]
			);

			if (float.Parse((string)eventInfo["probability"]) > 0) 
				++weighted;

			newEvent.PrintEvent();

			eventList[i] = newEvent;
		}

		return eventList;
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Nada
	}
}
