extends RichTextLabel

var char_width_px = 20
var num_rows = 5

var top_border = "=="
var space = ""

func generate_border(width_px):

	top_border = "=="
	space = ""
	
	var new_border = ""
	
	for i in range(width_px/char_width_px - 2):
		top_border += "="
		space += " "
	
	new_border = top_border
	
	for i in range(num_rows):
		new_border += "\n|" + space + "|"	
	
	new_border += "\n" + top_border

	self.text = new_border


# Called when the node enters the scene tree for the first time.
func _ready():
	var win_width = get_viewport().size.x
	generate_border(win_width)
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var win_width = get_viewport().size.x
	generate_border(win_width)
