extends WarperState
class_name WarperIdle

@export var shaker: ShakerComponent2D
var tween: Tween
func on_enter():
	if tween: tween.kill()
	tween = TweenUtils.create_tween(self, tween.EaseType.EASE_IN_OUT, tween.TransitionType.TRANS_EXPO)
	tween.tween_interval(1.0)
	tween.tween_callback(transitioned.emit.bind("first"))
	
func on_exit():
	pass

func physics_tick(_delta: float):
	print("A")

func tick(_delta: float):
	pass
