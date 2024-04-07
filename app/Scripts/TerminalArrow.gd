extends RichTextLabel

var pt_size = 32
var char_width_px = 20
onready var dialogue_box = get_parent()

func update_position():
	self.rect_position.x = 2*char_width_px
	self.rect_position.y = get_viewport().size.y - 2*pt_size

# Called when the node enters the scene tree for the first time.
func _ready():
	self.rect_size.x = 50
	self.rect_size.y = 50
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	update_position()
	pass
