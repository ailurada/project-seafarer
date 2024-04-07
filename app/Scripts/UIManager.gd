extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

export var map_path: NodePath
var map: Node

export var user_input_path: NodePath
var user_input: Node

export var dialogue_box_path: NodePath
var dialogue_box: Node

var current_selection = -1
var map_event = true

# Called when the node enters the scene tree for the first time.
func _ready():
	map = get_node(map_path)
	user_input = get_node(user_input_path)
	dialogue_box = get_node(dialogue_box_path)
	

func _process(delta):
	var new_selection = user_input.get_input_selection()
	if new_selection != current_selection:
		current_selection = new_selection
		dialogue_box.highlight_option(current_selection)
		if map_event:
			map.set_hover(current_selection)
			map.redraw_map()
		
	if (Input.is_action_just_released("enter")):
		user_input.clear_command()
		if map_event:
			map.disable()
			map_event = false

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

# Draws the map on the screen
func draw_map(map_string: String, adjacent_nodes: Array, numRows: int, numCols: int):
	map.draw_map(map_string, adjacent_nodes, numRows, numCols)
	map_event = true

func draw_event(description: String, options: Array):
	dialogue_box.show_dialogue(description, options)
	if map_event:
		dialogue_box.set_box_location(dialogue_box.RIGHT)
	else:
		dialogue_box.set_box_location(dialogue_box.BOTTOM)
