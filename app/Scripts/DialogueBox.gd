extends Label

enum {BOTTOM, RIGHT}
var location = BOTTOM

var char_width_px = 20
var num_rows = 8
var pt_size = 32

var top_border = "=="
var space = ""

onready var dialogue_box_text = $DialogueBoxText

onready var options = []

# helper func to calculate width of dialogue box in units of char-widths
func get_desired_width():
	if self.location == BOTTOM:
		return get_viewport().size.x/char_width_px
	elif self.location == RIGHT:
		var box_width_px = get_viewport().size.x/3
		return box_width_px/char_width_px

# helper func to calculate number of lines in dialogue box
func calc_num_lines():
	return self.dialogue_box_text.get_line_count() + len(options) + 1

# generate string for dialogue box
func generate_border():

	top_border = "=="
	space = ""
	
	var new_border = ""
	
	var width_char = get_desired_width() 
	
	for i in range(width_char - 2):
		top_border += "="
		space += " "
	
	new_border = top_border
	
	num_rows = calc_num_lines()
	
	for i in range(num_rows):
		new_border += "\n|" + space + "|"	
	
	new_border += "\n" + top_border
	
	self.set_text(new_border)

# update hitbox
func update_size():
	self.rect_size.x = len(top_border)*char_width_px
	self.rect_size.y = (self.get_line_count())*(pt_size+3)
		
# put dialogue box at bottom/right
func update_position(win_width, win_height):
	if location == BOTTOM:
		self.rect_position.x = (win_width - self.rect_size.x)/2
		self.rect_position.y = win_height - self.rect_size.y - pt_size*2
	
	elif location == RIGHT:
		self.rect_position.x = win_width - self.rect_size.x - char_width_px*2
		self.rect_position.y = 0 + pt_size

func update_option_positions():
	var length = len(self.options)
	var init_pos = self.rect_size.y - 2.75*pt_size
	for i in range(length):
		self.options[i].rect_position.x = 0 + char_width_px*4
		self.options[i].rect_position.y = init_pos - (pt_size + 14)*(length-i-1)

# set box to be on bottom (event) or right (map)
# location should be DialogueBox.BOTTOM or DialogueBox.RIGHT
func set_box_location(location):
	self.location = location

# call to show dialogue box with given description and options
func show_dialogue(description, option_strings):
	self.dialogue_box_text.set_text(description)
	
	# create options objects
	for i in range(len(option_strings)):
		self.options.append(RichTextLabel.new())
		self.options[i].bbcode_enabled = true
		self.options[i].set_bbcode(option_strings[i])
		
		self.options[i].rect_size.x = 500
		self.options[i].rect_size.y = 50
		add_child(options[i])
		
	update_option_positions()
		
	self.show()
# call to hide dialogue when no longer needed (cleanup)

func hide_dialogue():
	self.text = ""
	self.hide()

# pick from options 1-4 to highlight, or pass 0 to reset all
func highlight_option(option_num):
	for i in range(len(options)):
		var clean_text = options[i].bbcode_text.trim_prefix("[color=lime]")
		options[i].set_bbcode(clean_text)
	
	if option_num != 0:
		options[option_num-1].set_bbcode("[color=lime]" + options[option_num].bbcode_text)

# Called when the node enters the scene tree for the first time.
func _ready():
	self.align = Label.ALIGN_CENTER
	var viewport_size = get_viewport().size
	var win_width = viewport_size.x
	var win_height = viewport_size.y
	generate_border()
	update_size()
	update_position(win_width, win_height)
	update_option_positions()
	show_dialogue("nys omeome funnyso  ome fuy s ome funnysome e funny", ["[1] alsdkfjal","[2] thing 1","[1] alsdkfjal","[2] thing 1"])
	highlight_option(0)	
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var viewport_size = get_viewport().size
	var win_width = viewport_size.x
	var win_height = viewport_size.y

	generate_border()
	update_size()
	update_option_positions()
	update_position(win_width, win_height)
	


