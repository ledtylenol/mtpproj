@tool
class_name BgShader extends ColorRect
@export var world: GameWorld

@export var time_scale: float

@export var tween_duration: float

@export var ease_type: Tween.EaseType

@export var trans_type: Tween.TransitionType

@export var area: Rect2
var time: float:
	set(v):
		time = v
		set_instance_shader_parameter("time", v)

var flow_dir: Vector2:
	set(v):
		flow_dir = v;
		set_instance_shader_parameter("flow_dir", v);

var target_dir: Vector2  
var target_time_scale: float;

var distance_threshold: float:
	set(v):
		distance_threshold = v;
		set_instance_shader_parameter("distance_threshold", v);

var tween: Tween 
var dist_tween: Tween 
func _ready() -> void:
	if world:
		world.play_area_updated.connect(update_size)
		update_size(world.play_area);
	else:
		update_size(area)
	tween_distance_threshold(1);
	tween_time_scale(0.2, Vector2.DOWN);

func _physics_process(delta: float) -> void:
	time += delta * time_scale;
	if (Engine.is_editor_hint()): return;
	flow_dir += target_dir * delta * time_scale;

func update_size(play_area: Rect2 ) -> void:
	size = play_area.size;
	position = -size / 2;

func tween_time_scale(new_time_scale: float , new_flow_dir: Vector2 ) -> void:
	if (target_dir == new_flow_dir): return;
	if tween: tween.kill();
	tween = create_tween().set_ease(ease_type).set_trans(trans_type).set_parallel();

	tween.tween_property(self, "time_scale", new_time_scale, tween_duration);
	tween.tween_property(self, "target_dir", new_flow_dir, tween_duration);

func tween_distance_threshold(new_distance_threshold: float ) -> void:
	if dist_tween: dist_tween.kill()
	dist_tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO).set_parallel();

	dist_tween.tween_property(self, "distance_threshold", new_distance_threshold, tween_duration / 2);

func reset_time() -> void:
	if (time > 3600):
		time = -100;
