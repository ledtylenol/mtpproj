class_name PointTrace extends State
var start_pos: Vector2 
@export var applier: PointApplier 
func on_enter() -> void:
	start_pos = entity.global_position;
	var tween = create_tween().set_ease(Tween.EaseType.EASE_IN).set_trans(Tween.TransitionType.TRANS_CUBIC);
	tween.tween_interval(randf_range(0.1, 0.9));
	tween.tween_method(lerp_to_target, 0.0, 1.0, 1.0);
	tween.tween_callback(applier.apply_points)
func lerp_to_target(weight: float) -> void:
	var player_pos = Global.player.global_position;
	entity.global_position = start_pos.lerp(player_pos, weight);
func on_exit():
	pass

func physics_tick(_delta: float):
	pass

func tick(_delta: float):
	pass
