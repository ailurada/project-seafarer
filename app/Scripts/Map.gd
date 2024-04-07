extends RichTextLabel


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

const YOU_ARE_HERE = "yellow"
const SELECTED = "lime"
const VISITED = "blue"
const NORMAL = "silver"

const GROUND = "gray"
const EDGE_COLOR = "white"

# Test cases
const edge_mapping = {"0": "_", "1": "|", "4": "\\", "5": "-", "7": "/"}

const test_map: String = "     __                      /\n    /  \\( )                 | \n   /  _/  40               (@)\n   \\_/      400         00075 \n               40   0007    4 \n              __ 4 7         5\n             /  \\(*)55555555( \n             \\__/ 1         --\n       _       007            \n    __/ \\( )555               "
const test_num_rows = 10
const test_num_cols = 30


# Called when the node enters the scene tree for the first time.
func _ready():
	#draw_map(test_map, [[1, 9], [2, 28], [6, 29], [9, 10]], test_num_cols, test_num_rows)
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

# counter var
var node_number = 1

# class variables
var map_str: String
var adj_nodes: Array
var num_rows: int
var num_cols: int

var hover_selection: int = 1

func disable():
	self.hide()

# Draws a map given a string representation and x, y dimensions
func draw_map(map_string: String, adjacent_nodes: Array, numRows: int, numCols: int):
	self.show()
	map_str = map_string
	adj_nodes = adjacent_nodes
	num_rows = numRows
	num_cols = numCols
	
	redraw_map()

func set_hover(selection: int):
	hover_selection = selection

func redraw_map():
	self.set_bbcode("")
	node_number = 1
	
	# Top horizontal line
	for i in range(num_cols + 2):
		self.append_bbcode("_")
	self.append_bbcode("\n")
	
	var map_str_array = map_str.split("\n")
	
	# Add lines row by row
	for n in range(num_rows):
		self.append_bbcode("|")
		_add_row(map_str_array[n], n)
		self.append_bbcode("|\n")
		
	# Bottom horizontal line
	for i in range(num_cols + 2):
		self.append_bbcode("‾")


# Adds a single row to the RichTextLabel
# given a string representation and the current row
func _add_row(row_str: String, curr_row: int):
	print(len(row_str))
	var str_length = len(row_str)
	var i = 0
	while i < str_length:
		# if it's a node
		if row_str[i] == "(":
			var curr_col = i + 1
			var color = NORMAL
			var center_char = " "
			
			# check if you have visited the node at (curr_row, curr_col) 
			if i < str_length - 1:
				if row_str[i+1] == "@":
					color = VISITED
				if row_str[i+1] == "*":
					color = YOU_ARE_HERE
					center_char = "*"
			
			# check if it is adjacent to the current node
			if [curr_row, curr_col] in adj_nodes:
				# check if you are selecting node at (curr_row, curr_col)
				center_char = String(node_number)
				
				if (hover_selection == node_number):
					color = SELECTED
					
				node_number += 1
				
			self.append_bbcode("[color=" + color + "]")
			self.append_bbcode(row_str[i])
			if i < str_length - 1:
				self.append_bbcode(center_char)
			if i < str_length - 2:
				self.append_bbcode(row_str[i+2])
			self.append_bbcode("[color=" + NORMAL + "]")
			i += 2
			
		# if it's an edge
		elif row_str[i] in edge_mapping:
			self.append_bbcode("[color=" + EDGE_COLOR + "]")
			self.append_bbcode(edge_mapping[row_str[i]])
			self.append_bbcode("[color=" + NORMAL + "]")
			
		# empty spaces
		elif row_str[i] == " ":
			self.append_bbcode(row_str[i])
		
		# ground chars
		else:
			self.append_bbcode("[color=" + GROUND + "]")
			self.append_bbcode(row_str[i])
			self.append_bbcode("[color=" + NORMAL + "]")
			
		i += 1
