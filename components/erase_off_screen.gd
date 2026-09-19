class_name EraseOffScreen extends Node
@export var target: Node 
@export var notifier: VisibleOnScreenNotifier2D 

func _ready() -> void:
	notifier.screen_exited.connect(erase)

func erase() -> void:
	target.queue_free();
