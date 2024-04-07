extends TextEdit



# Declare member variables here. Examples:
# var a = 2
# var b = "text"
export var text_output_path: NodePath
var text_output: RichTextLabel
var start_position: Vector2

export var line_width: int



# Called when the node enters the scene tree for the first time.
func _ready():
	text_output = get_node(text_output_path)
	start_position = self.rect_position
	
	#var dynamic_font = DynamicFont.new()
	#dynamic_font.font_data = load("res://Fonts/JetBrainsMono-Regular.ttf")
	#dynamic_font.size = 12
	#self.set("custom_fonts/font", dynamic_font)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_released("enter"):
		execute_command()
			
func execute_command():
	var command = see_command()
	
	## temp
	text_output.text = command
	self.text = ""
	self.rect_position += Vector2(0, line_width)
	##
	
	return command
	
func see_command():
	return self.text.substr(0, self.text.length() - 1) + "\n> "
