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
	SeaNode[] LoadSeaNodes(string filePath, out int amount) {
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

			int[] adjList = new int[adjCount + 1];

			for (; j < adjCount; ++j)
				adjList[j] = Int32.Parse((string)adjArray[j]);

			adjList[j] = -1;

			SeaNode newNode = new SeaNode(
				i,
				Int32.Parse((string)nodeInfo["eid"]),
				(string)nodeInfo["title"],
				(string)nodeInfo["description"],
				adjList,
				(bool)nodeInfo["visited"]
			);

			nodeList[i] = newNode;
		}

		return nodeList;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int numNodes = 0;
		var nodes = LoadSeaNodes("res://data/nodes.json", out numNodes);

		for (int i = 0; i < numNodes; ++i) {
			nodes[i].printNode();
		}

	}
}
