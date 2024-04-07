extends RichTextLabel

var chars_per_update = 1


# sets text to be displayed in the dialogue box
func set_text(text):
	self.text = text

func update_size():
	var border_size = get_parent().rect_size
	self.rect_size.x = border_size.x - 10
	self.rect_size.y = border_size.y - 10

# Called when the node enters the scene tree for the first time.
func _ready():
	self.text  = "something or other. some more stuff and stuff"
	
	# start with text not visible
	self.visible_characters = 0;
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	self.visible_characters += chars_per_update
	update_size()
	self.set_position(get_parent().rect_position)
	print(get_parent().rect_position)
	self.anchor_top = 0.5
	self.anchor_bottom = 0.5
	self.anchor_left = 0.5
	self.anchor_right = 0.5
	
	self.margin_top = -self.rect_size.y / 2
	self.margin_bottom = -self.rect_size.y / 2
	self.margin_left = -self.rect_size.x / 2
	self.margin_right = -self.rect_size.x / 2
	
