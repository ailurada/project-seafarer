using System;
using System.IO;
using System.Text.Json;

// Reads from node.json, and initializes data for nodes and edges.
void GetNodeData(out nodeArray, out adjacencyList)
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
					}
					if (!node.TryGetProperty("eventId", out JsonElement eId)) {
						// Error!
					}
					if (!node.TryGetProperty("description", out JsonElement description)) {
						// Error!
					}
					if (!node.TryGetProperty("visited", out JsonElement visited)) {
						// Error!
					}
					if (!node.TryGetProperty("name", out JsonElement name)) {
						// Error!
					}
					// Initialize node array according to the data read.
					nodeArray[index] = Node(id.GetUInt32(), eId.GetUInt32(), visited.GetBooolean(), name.GetString(), description.GetString());
				}
			}
		}
		// There was some exception. Error.
		catch (JsonException exception) {
			// Error!
		}
	}
	// File not found. Error.
	else {
		// Error!
	}
	
	return;
}
