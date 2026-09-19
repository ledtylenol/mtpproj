@abstract class_name DiggerState extends State
var digger: Digger

func _ready() -> void:
	super()
	digger = owner as Digger;
