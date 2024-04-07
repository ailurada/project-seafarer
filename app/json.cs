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

		JSONParseResult jsonResult = JSON.Parse(rawJSON);
		var dict = jsonResult.Result as Godot.Collections.Dictionary;
		
		return dict;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var dict = readJSON("res://data/test.json");
			
		GD.Print(dict["status"]);
	}
}
