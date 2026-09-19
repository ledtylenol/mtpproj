class_name BgHandler extends Node

@export var shaders: Array[BgShader] = [];
@export var level_handler: LevelHandler
var tween: Tween 
var distance_tween: Tween 
var dir_tween: Tween 

var heat: float 
var time_scale:  float:
	get:
		return shaders[0].time_scale

func _ready() -> void:
	level_handler.boss_next_turn.connect(tween_time_scale.bind(0.001, 1));
	level_handler.boss_next_turn.connect(tween_dir.bind(Vector2.UP, 1.0));

	Global.player.health.died.connect(func(_e): tween_time_scale(0.005, 5))
	Global.player.health.died.connect(func(_e): tween_dir(Vector2.RIGHT, 5))
	level_handler.boss_enemy_killed.connect(add_heat)
	level_handler.level_finished.connect(reset_time)

	level_handler.level_finished.connect(set.bind("heat", 0))
	level_handler.level_finished.connect(tween_time_scale.bind(0.05, 0.5));
	level_handler.level_finished.connect(tween_dir.bind(Vector2.DOWN, 1.0));

	level_handler.level_started.connect(tween_if_not_boss);
	PauseManager.paused.connect(tween_primary_bg)
	PauseManager.un_paused.connect(untween_primary_bg)

func tween_if_not_boss():
	if (!level_handler.is_boss()):
		tween_time_scale(0.2, 0.5); 
func add_heat() -> void:
	heat += 0.2;
	tween_time_scale(heat, 1);
func reset_time() -> void:
	for shader in shaders: shader.reset_time();
func tween_time_scale(new_scale: float, duration: float) -> void:
	if tween: tween.kill()

	tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_CUBIC).set_parallel();
	for shader in shaders:
		tween.tween_property(shader, "time_scale", new_scale, duration);

func tween_time_scale_from(new_scale: float, duration: float, add: float) -> void:
	if tween: tween.kill()

	tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_CUBIC).set_parallel();
	for shader in shaders:
		tween.tween_property(shader, "time_scale", new_scale, duration).from(add);

func tween_dir(new_dir: Vector2, duration: float) -> void:
	if dir_tween: dir_tween.kill()

	dir_tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_CUBIC).set_parallel();
	for shader in shaders:
		dir_tween.tween_property(shader, "target_dir", new_dir, duration);

func untween_primary_bg() -> void:
	if distance_tween: distance_tween.kill()
	distance_tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO);

	var primary_bg = shaders.front();

	distance_tween.tween_property(primary_bg, "distance_threshold", 1.0, 1.0);

func tween_primary_bg() -> void:
	if distance_tween: distance_tween.kill()
	distance_tween = create_tween().set_ease(Tween.EaseType.EASE_IN_OUT).set_trans(Tween.TransitionType.TRANS_EXPO);

	var primary_bg = shaders.front();

	distance_tween.tween_property(primary_bg, "distance_threshold", 10.0, 10.0);
