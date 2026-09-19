class_name PlayerCamera extends Camera2D
@export var player: Player
@export var tween_intensity_curve: Curve  
@export var lerp_influence_over_dist: Curve  
@export var fixed := true

var zoom_tween: Tween 
var tween: Tween 

var time_since_last_tween := 0.0;
func _ready() -> void:
	await World.single.scene_load_ended
	LevelHandler.current.level_finished.connect(tween_zoom)
	Global.boss_phase_changed.connect(tween_zoom)
func _physics_process(delta: float) -> void:
	time_since_last_tween += delta;
	if not fixed:
		var dist := global_position.distance_to(player.global_position)
		global_position = M.smooth_nudge(global_position, player.global_position, lerp_influence_over_dist.sample_baked(dist), delta)
	else:
		global_position = M.smooth_nudge(global_position, Vector2.ZERO, 5.0, delta)

func tween_zoom() -> void:
	if zoom_tween: zoom_tween.kill();
	zoom_tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO);
	zoom_tween.tween_property(self, "zoom", Vector2.ONE, 1.0).from(Vector2.ONE * tween_intensity_curve.sample_baked(time_since_last_tween));
	time_since_last_tween = 0;
	reset_physics_interpolation();
