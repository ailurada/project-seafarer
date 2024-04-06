extends RichTextLabel

var chars_per_update = 1

# Called when the node enters the scene tree for the first time.
func _ready():
	self.text  = "something or other. some more stuff and stuff"
	
	# center in border 
	self.anchor_left = 0.5
	self.anchor_right = 0.5
	self.anchor_top = 0.5
	self.anchor_bottom = 0.5
	
	
	# start with text not visible
	self.visible_characters = 0;
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	self.visible_characters += chars_per_update

# sets text to be displayed in the dialogue box
func set_text(text):
	self.text = text
	
