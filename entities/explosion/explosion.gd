class_name Explosion extends Entity
func _init(_stats: ExplosionStats ,  _mask: int, _layer: int, _clear_on_level_finish: bool) -> void:
	stats = _stats
	clear_on_level_finish = _clear_on_level_finish
	mask = _mask
	layer = _layer
@export var clear_on_level_finish: bool
@export var stats: ExplosionStats

var color := Color.RED
var mask: int
var layer: int
var wind_up: AudioStream = load("res://entities/explosion/windup.sfxr");
var boom: AudioStream = load("res://entities/explosion/boom.sfxr");
var ratio: float:
	set(v):
		ratio = v
		queue_redraw()

var init_ratio: float:
	set(v):
		init_ratio = v
		queue_redraw()

var active := false 

var tween: Tween 
var hit_box: HitBox
var collision_shape: CollisionShape2D
var shape: CircleShape2D
var wind_up_player: AudioStreamPlayer
var boom_player: AudioStreamPlayer

signal visual_done();

signal explosion_done();
@warning_ignore_start("shadowed_variable")
func _ready() -> void:
	var tween = stats.get_and_start_tween(self);

	tween.tween_callback(explosion_done.emit);
	tween.tween_callback(queue_free);
	hit_box = HitBox.new()
	hit_box.active = false

	wind_up_player = AudioStreamPlayer.new()
	wind_up_player.stream = wind_up
	wind_up_player.bus = "Sfx"

	boom_player = AudioStreamPlayer.new()
	boom_player.stream = boom
	boom_player.bus = "Sfx"

	collision_shape = CollisionShape2D.new();
	shape = CircleShape2D.new()
	shape.radius = 0
	collision_shape.shape = shape;

	hit_box.damage = stats.damage;
	hit_box.collision_mask = mask;
	hit_box.collision_layer = layer;

	hit_box.add_child(collision_shape);

	if (clear_on_level_finish):
		LevelHandler.current.level_finished.connect(queue_free);
	add_child(wind_up_player);
	if (stats.init_duration > 0):
		wind_up_player.play();
	add_child(boom_player);
	add_child(hit_box);

func _draw() -> void:
	var radius := stats.radius(ratio);
	var visual_radius := stats.radius(init_ratio);
	var inner_width := stats.inner_width(init_ratio);
	var visual_width := stats.outer_width(ratio);
	var outer_width := stats.outer_width(init_ratio);
	draw_circle(Vector2.ZERO, radius, color, false, visual_width);
	draw_circle(Vector2.ZERO, visual_radius, Color.WHITE, false, outer_width);
	draw_circle(Vector2.ZERO, visual_radius, color, false, inner_width);

func _physics_process(_delta: float) -> void:
	shape.radius = stats.radius(ratio) - stats.outer_width(ratio) / 2;
	wind_up_player.pitch_scale = 1 - init_ratio * init_ratio / 1.2;
	wind_up_player.volume_linear = 1 - init_ratio * init_ratio;
	if (ratio > stats.active_width_threshold && active):
		active = false;
		hit_box.active = false;

func activate() -> void:
	hit_box.active = true;
	active = true;
	boom_player.play();
	visual_done.emit();
