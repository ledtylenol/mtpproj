class_name PointApplier extends Node
@export var point: Point 
var active: bool = true;

func apply_points() -> void:
	Global.run.score += point.points;
	#TODO! Add sound here
	owner.queue_free();
	SoundHandler.hit();
	Global.score_changed.emit(int(Global.run.score))
