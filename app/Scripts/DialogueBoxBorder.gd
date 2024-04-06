extends RichTextLabel

var top_border = "================================================================================"
var space = ""

# Called when the node enters the scene tree for the first time.
func _ready():
	for i in range(len(top_border) -2):
		space += " "
		
	self.text = top_border + "\n|" + space + "|\n|" + space + "!\n" + top_border


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
