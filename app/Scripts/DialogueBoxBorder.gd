extends Label

var char_width_px = 20
var num_rows = 5
var pt_size = 30

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
	
func update_size():
	self.rect_size.x = len(top_border)*char_width_px
	self.rect_size.y = len(self.text)/len(top_border)*pt_size + 80

# put dialogue box at bottom
func update_position(win_width, win_height):
	self.rect_position.x = (win_width - self.rect_size.x)/2
	self.rect_position.y = win_height - self.rect_size.y - pt_size - 15
	

# Called when the node enters the scene tree for the first time.
func _ready():
	var win_width = get_viewport().size.x
	generate_border(win_width)
	self.align = Label.ALIGN_CENTER
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var viewport_size = get_viewport().size
	var win_width = viewport_size.x
	var win_height = viewport_size.y
	
	generate_border(win_width)
	update_size()
	update_position(win_width, win_height)


