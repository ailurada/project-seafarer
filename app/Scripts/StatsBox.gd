extends Label

enum {MAP, SHOP}
var location = MAP

var char_width_px = 20
var num_rows = 8
var pt_size = 32
const NUM_STATS = 3

var top_border = "=="
var space = ""

export var dialogue_box_path: NodePath
var dialogue_box: Node

onready var dialogue_box_text = $StatsBoxText

onready var options = []

# helper func to calculate width of dialogue box in units of char-widths
func get_desired_width():
	var box_width_px = get_viewport().size.x/3
	return box_width_px/char_width_px

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
	
	num_rows = NUM_STATS
	
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
	if location == MAP:
		self.rect_position.x = dialogue_box.rect_position.x
		self.rect_position.y = dialogue_box.rect_position.y + dialogue_box.rect_size.y
	
	elif location == SHOP:
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

func initialize_options(option_strings):

	for i in range(len(option_strings)):
		self.options.append(RichTextLabel.new())
		self.options[i].bbcode_enabled = true
		self.options[i].set_bbcode("[" + String(i+1) + "] " + option_strings[i])

		self.options[i].rect_size.x = 500
		self.options[i].rect_size.y = 50
		add_child(options[i])
		
# call to show dialogue box with given description and options
func show_dialogue(description, option_strings):
	self.dialogue_box_text.set_text(description)
	
	initialize_options(option_strings)
	update_option_positions()
		
	self.show()

# call to hide dialogue when no longer needed (cleanup)
func hide_dialogue():
	for i in range(options):
		options[i].queue_free()
	
	self.options = []
	self.hide()

# pick from options 1-4 to highlight, or pass 0 to reset all
func highlight_option(option_num):
	for i in range(len(options)):
		var clean_text = options[i].bbcode_text.trim_prefix("[color=lime]")
		options[i].set_bbcode(clean_text)
	
	if option_num > 0 && option_num <= len(options):
		options[option_num-1].set_bbcode("[color=lime]" + options[option_num-1].bbcode_text)

# Called when the node enters the scene tree for the first time.
func _ready():
	dialogue_box = get_node(dialogue_box_path)
	
	self.align = Label.ALIGN_CENTER
	var viewport_size = get_viewport().size
	var win_width = viewport_size.x
	var win_height = viewport_size.y
	generate_border()
	update_size()
	update_position(win_width, win_height)
	update_option_positions()
	show_dialogue("nys omeome funnyso  ome fuy s ome funnysome e funny", ["alsdkfjal","aa 1","alsdkfjal","thing 1"])
	highlight_option(2)	
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var viewport_size = get_viewport().size
	var win_width = viewport_size.x
	var win_height = viewport_size.y

	generate_border()
	update_size()
	update_option_positions()
	update_position(win_width, win_height)
	


