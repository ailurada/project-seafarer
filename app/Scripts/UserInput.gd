extends TextEdit

export var text_output_path: NodePath
var text_output: RichTextLabel
var start_position: Vector2

export var line_width: int

var pt_size = 32
var char_width_px = 20
onready var dialogue_box = get_parent()

func update_position():
	var init_pos = dialogue_box.rect_size.y - 2.75*pt_size
	self.rect_position.x = dialogue_box.rect_position.x + 4*char_width_px
	self.rect_position.y = init_pos + 2.75*pt_size
	
# Called when the node enters the scene tree for the first time.
func _ready():
	text_output = get_node(text_output_path)
	start_position = self.rect_position
	self.rect_size.x = get_viewport().size.x
	self.rect_size.y = pt_size + 20
	
	
	#var dynamic_font = DynamicFont.new()
	#dynamic_font.font_data = load("res://Fonts/JetBrainsMono-Regular.ttf")
	#dynamic_font.size = 12
	#self.set("custom_fonts/font", dynamic_font)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update_position()
	#if Input.is_action_just_released("enter"):
	#		text_output.text += self.text.substr(0, self.text.length() - 1) + "\n> "
	#		self.text = ""
	#		self.rect_position += Vector2(0, line_width)
