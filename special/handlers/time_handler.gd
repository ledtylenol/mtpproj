extends Node

var tween: Tween
func stop(time: float, delay: float) -> void:
	if tween: tween.kill();
	tween = create_tween().set_ignore_time_scale().set_ease(Tween.EaseType.EASE_IN).set_trans(Tween.TransitionType.TRANS_EXPO);
	Engine.time_scale = 0;
	tween.tween_interval(delay);
	tween.tween_property(Engine, "time_scale", 1.0, time);

func resume(duration: float) -> void:
	if tween: tween.kill();
	tween = create_tween().set_ignore_time_scale().set_ease(Tween.EaseType.EASE_IN).set_trans(Tween.TransitionType.TRANS_EXPO);
	tween.tween_property(Engine, "time_scale", 1.0, duration);
