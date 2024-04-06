extends TextEdit



# Declare member variables here. Examples:
# var a = 2
# var b = "text"
export var text_output_path: NodePath
var text_output

# Called when the node enters the scene tree for the first time.
func _ready():
	text_output = get_node(text_output_path)


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func _input(event):
	if event is InputEventKey and event.pressed:
		if event.scancode == KEY_ENTER:
			text_output.text += self.text + "\n> "
			self.text = ""
