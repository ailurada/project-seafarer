extends TextEdit

var start_position: Vector2

export var line_width: int

var pt_size = 32
var char_width_px = 20
onready var dialogue_box = get_parent()


func update_position():
	self.rect_position.x = 4*char_width_px
	self.rect_position.y = get_viewport().size.y - 2*pt_size
	self.rect_size.x = get_viewport().size.x - 10*char_width_px
	
# Called when the node enters the scene tree for the first time.
func _ready():
	start_position = self.rect_position
	self.rect_size.x = get_viewport().size.x - 10*char_width_px
	self.rect_size.y = pt_size + 20
	self.caret_block_mode = true
	self.context_menu_enabled = false
	self.caret_blink = true
	self.caret_blink_speed = 0.5
	call_deferred("grab_focus")
	
	#var dynamic_font = DynamicFont.new()
	#dynamic_font.font_data = load("res://Fonts/JetBrainsMono-Regular.ttf")
	#dynamic_font.size = 12
	#self.set("custom_fonts/font", dynamic_font)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update_position()
	call_deferred("grab_focus")
		
# returns the last command from the terminal w/ out extra line break	
func clear_command():
	self.text = ""

# returns what the user has typed in the terminal	
func get_input_selection():
	return int(self.text)
