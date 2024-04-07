extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func draw_map(map_string, xDim, yDim):
	self.text = "|"
	for i in range(xDim):
		map_string += "-"
	self.text += "|\n"
	
	var map_str_array = map_string.split("\n")
	
	for n in range(yDim):
		self.text += "|"
		self.text += map_str_array[n]
		self.text += "|\n"
		
	self.text = "|"
	for i in range(xDim):
		map_string += "-"
	self.text += "|"
