// json.cs
//
// Implements JSON functions and the ability to 
// load SeaNodes and events from files.

using Godot;
using System;

public class json : Node
{
	// Given a filePath, read the JSON from the file and return the 
	// JSON as a dictionary.
	Godot.Collections.Dictionary readJSON(string filePath) {
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
		Godot.Collections.Dictionary dict = readJSON(filePath);
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

	public Event[] LoadEvents(string filePath, out int amount) {
		Godot.Collections.Dictionary dict = readJSON(filePath);
		int amtEvents = dict.Count;
		amount = amtEvents;
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

			// Translate choice destinations to a string array
			var destArr = eventInfo["choice_destinations"] as Godot.Collections.Array;

			int numDests = destArr.Count;
			int[] destList = new int[numDests];

			j = 0;
			for (; j < numDests; ++j) {
				destList[j] = Int32.Parse((string)destArr[j]);
			}

			Event newEvent = new Event(
				i,
				(string)eventInfo["title"],
				(string)eventInfo["description"],
				float.Parse((string)eventInfo["probability"]),
				descList,
				destList,
				int.Parse((string)eventInfo["delta_food"]),
				int.Parse((string)eventInfo["delta_gold"]),
				int.Parse((string)eventInfo["delta_health"])
			);

			eventList[i] = newEvent;
		}

		return eventList;
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int numNodes = 0;
		int numEvents = 0;

		var nodes = LoadSeaNodes("res://data/nodes.json", out numNodes);
		var events = LoadEvents("res://data/events.json", out numEvents);

		//for (int i = 0; i < numEvents; ++i)
		//	events[i].printEvent();

	}
}
