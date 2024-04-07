extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

onready var map = $Map
onready var user_input = $TerminalInput
onready var dialogue_box = $DialogueBox
onready var stats_box = $StatsBox
onready var event_img = $EventImage

export var game_manager_path: NodePath
var game_manager: Node

var default_image = ""

var current_selection = -1
var map_event = false

# Called when the node enters the scene tree for the first time.
func _ready():
	game_manager = get_node(game_manager_path)
	

func _process(delta):
	var new_selection = user_input.get_input_selection()
	if new_selection != current_selection:
		current_selection = new_selection
		dialogue_box.highlight_option(current_selection)
		if map_event:
			map.set_hover(current_selection)
			map.redraw_map()
		
	if (Input.is_action_just_released("enter")):
		if map_event:
			map_event = false
			map.disable()
			
		game_manager.UserInput(current_selection)
		user_input.clear_command()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

# Draws the map on the screen
func draw_map(map_string: String, adjacent_nodes: Array, top: int, left: int):
	var adjacent_node_coords = []
	var adjacent_node_names = []
	for node_id in adjacent_nodes:
		adjacent_node_coords.append(game_manager.NodeCoordinates(node_id))
		adjacent_node_names.append(game_manager.NodeName(node_id))
		
	var numRows	= game_manager.gridHeight
	var numCols = game_manager.gridWidth
	map.draw_map(map_string, adjacent_node_coords, numRows, numCols, top, left)
	map_event = true
	draw_event("Travel", "Where would you like to go next?", adjacent_node_names, "")


func draw_event(title: String, description: String, options: Array, img_str: String):
	dialogue_box.show_dialogue(title + ":\n" + description, options)
	if map_event:
		event_img.hide_image()
		dialogue_box.set_box_location(dialogue_box.RIGHT)
		stats_box.set_box_location(stats_box.MAP)
	else:
		dialogue_box.set_box_location(dialogue_box.BOTTOM)
		stats_box.set_box_location(stats_box.SHOP)
		
	if img_str != "":
		if img_str == "default":
			event_img.show_image(default_image)
		else:
			event_img.show_image(img_str)

func draw_stats():
	stats_box.show_stats([game_manager.Health, game_manager.Food, game_manager.Money])
