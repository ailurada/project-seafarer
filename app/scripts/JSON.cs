


void GetNodeData(out nodeArray, out adjacencyList)
{
	string filePath = "node.json";
	
	if (File.Exists(filePath)) {
		string jsonString = File.ReadAllText(filePath);
		
		try {
			JsonDocument jsonDocument = JsonDocument.Parse(jsonString);
			JsonElement root = jsonDocument.RootElement;
			
			int index = 0;
			while (true) {
				if (root.TryGetProperty(index.ToString(), out JsonElement node)) {
					
					if (root.TryGetProperty("id", out JsonElement id)) {
					}
					if (root.TryGetProperty("description", out JsonElement description)) {
					}
					
					++index;
				}
				else {
					break;
				}
			}
			
			
		}
		catch (JsonException exception) {
			// Error!
		}
	}
	else {
		// Error!
	}
	
	
}
