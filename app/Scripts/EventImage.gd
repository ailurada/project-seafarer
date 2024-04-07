extends RichTextLabel

const char_width_px = 20
const num_rows = 8
const pt_size = 32
const IMG_SIZE = 19

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

export var dialogue_box_path: NodePath
var dialogue_box: Node

const edge_mapping = {'0': "_", '1': "|", '4': "\\", '5': "-", '7': "/"}
const GROUND = "white"
const EDGE_COLOR = "gray"

func show_image(image_str: String):
	self.show()
	self.set_bbcode(_parse_string(image_str))

func hide_image():
	self.hide()

# Called when the node enters the scene tree for the first time.
func _ready():
	dialogue_box = get_node(dialogue_box_path)
	
# put above dialogue box
func _update_position(win_width, win_height):
	self.rect_position.x = dialogue_box.rect_position.x
	self.rect_position.y = dialogue_box.rect_position.y - (IMG_SIZE + 1) * pt_size
	
	
func _update_size():
	self.rect_size.x = dialogue_box.rect_size.x
	self.rect_size.y = IMG_SIZE * pt_size


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var viewport_size = get_viewport().size
	var win_width = viewport_size.x
	var win_height = viewport_size.y
	_update_position(win_width, win_height)
	_update_size()
	
func _parse_string(img: String):	
	var parsed_str = ""
	var i = 0
	var length = len(img)
	while i < length:
		var _char = img[i]
		if _char == " ":
			parsed_str += _char
		elif _char in edge_mapping:
			parsed_str += "[color=" + EDGE_COLOR + "]" + edge_mapping[_char]
		elif _char == "[":
			var seen = 0
			while seen < 2:
				parsed_str += img[i]
				if (img[i] == "]"):
					seen += 1
				i += 1
		else:
			parsed_str += "[color=" + GROUND + "]" + _char
		
		i+=1
	
	return parsed_str
