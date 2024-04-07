extends Label

var chars_per_update = 1
var char_width_px = 20
var pt_size = 30

# sets text to be displayed in the dialogue box
func set_text(text):
	self.text = text

func update_size(border_size):
	self.rect_size.x = border_size.x - char_width_px*6 - 10
	self.rect_size.y = border_size.y - pt_size*2
	
func update_position():
	self.rect_position.x = 0 + char_width_px*4
	self.rect_position.y = 0 + pt_size*1.5

# Called when the node enters the scene tree for the first time.
func _ready():
	self.text  = "something or other. some more stuff and stuff. and also some stuffsomething or other. somesomething or other. somesomething or other. some"

	self.autowrap = true
	
	# start with text not visible
	self.visible_characters = 0;
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	
	# scrolling text
	self.visible_characters += chars_per_update
	
	var border_size = get_parent().rect_size
	update_size(border_size)
	update_position()

	
	
	

	
