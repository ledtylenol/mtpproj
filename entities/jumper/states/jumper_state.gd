@abstract class_name JumperState extends State
var jumper: Jumper 

func _ready() -> void:
	super()
	jumper = owner as Jumper;
