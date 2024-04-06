extends RichTextLabel

var chars_per_update = 1

# Called when the node enters the scene tree for the first time.
func _ready():
	self.text  = "something or other. some more stuff and stuff"
	self.visible_characters = 0;

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	self.visible_characters += chars_per_update
