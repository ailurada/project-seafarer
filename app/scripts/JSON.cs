// json.cs
//
// Implements functions related to the storing and writing of data 
// to json files in the ../data/*.json files.

using System;
using System.IO;
using System.Text.Json;

// GetNodeData(out Node[], out Node[])
// Reads from node.json, and initializes data for nodes and edges.
// ============================================= 
// nodeArray: 	  The array to fill with the JSON data received
// adjacencyList: The adjacent nodes to the nodes being filled
void GetNodeData(out Node[] nodeArray, out Node[] adjacencyList)
{
	// Open the file
	string filePath = "node.json";
	if (File.Exists(filePath)) {
		string jsonString = File.ReadAllText(filePath);
		
		try {
			// Read and parse all of the text in file to the root object.
			JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
			JsonElement root = jsonDocument.RootElement;
			
			// Loop the amount of nodes present.
			int length = root.GetArrayLength();
			nodeArray = new Node[length];
			adjacencyList = new NodeId[length, MAX_ADJACENT];
			
			for (int index = 0; index < length; ++index) {
				// Get the node data as a JsonElement.
				if (root.TryGetProperty(index.ToString(), out JsonElement node)) {
					// Get the properties of the node. If some property is not found, error.
					if (!node.TryGetProperty("id", out JsonElement id)) {
						// Error!
						return -1;
					}
					if (!node.TryGetProperty("eventId", out JsonElement eId)) {
						// Error!
						return -1;
					}
					if (!node.TryGetProperty("description", out JsonElement description)) {
						// Error!
						return -1;
					}
					if (!node.TryGetProperty("visited", out JsonElement visited)) {
						// Error!
						return -1;
					}
					if (!node.TryGetProperty("name", out JsonElement name)) {
						// Error!
						return -1;
					}
					// Initialize node array according to the data read.
					nodeArray[index] = Node(id.GetUInt32(), eId.GetUInt32(), visited.GetBooolean(), name.GetString(), description.GetString());
				}
			}
		}
		// Some exception was found - most likely missing some data.
		catch (JsonException exception) {
			// Error!
			return -1;
		}
	}
	// File not found. Error.
	else {
		// Error!
		return -1;
	}
	
	return;
}
