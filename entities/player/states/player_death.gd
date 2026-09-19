class_name PlayerDeath extends PlayerState
@export var shaker_node: ShakerComponent2D  

@export var explosion_stats: ExplosionStats  

@export var final_explosion_stats: ExplosionStats  

@export var explosion_count: int  

@export var explosion_delay: float  

@export var initial_explosion_delay: float  

var death_tween: Tween 
func on_enter() -> void:
	tween_death();

func on_exit() -> void:
	shaker_node.force_stop_shake();
	shaker_node.intensity = 1

func physics_tick(_delta: float) -> void:
	pass

func tick(_delta: float) -> void:

	if (death_tween.is_running()): return;
	if (Input.is_action_just_pressed("proceed")):
		Global.world.reset();
		transitioned.emit("idle")

func tween_death() -> void:
	if death_tween: death_tween.kill()
	shaker_node.play_shake()
	death_tween = create_tween().set_ignore_time_scale().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO);
	death_tween.tween_interval(initial_explosion_delay);
	var delay := explosion_delay;
	for i in randi() % 3 + explosion_count:
		var subt = create_tween().set_ignore_time_scale();
		subt.tween_callback(spawn_explosion.bind(i));
		death_tween.tween_subtween(subt).set_delay(delay);

	death_tween.tween_callback(spawn_final_explosion);
	death_tween.tween_callback(player.hide);
	death_tween.tween_property(shaker_node, "intensity", 0, 0.5).from(1);

func spawn_explosion(i: int) -> void:
	var i_real: float = float(i) / explosion_count;
	var pos_offset: float = i_real * 50;

	var x = randf_range(-pos_offset, pos_offset);
	var y = randf_range(-pos_offset, pos_offset);

	var pos = Vector2(x, y);
	var explosion = Explosion.new(explosion_stats, 1 << 1, 1 << 2, false)
	explosion.position = pos + player.position
	explosion.scale = Vector2(1 + i_real, 1 + i_real)

	Global.spawn(explosion);
func spawn_final_explosion() -> void:
	var explosion = Explosion.new(final_explosion_stats, 1 << 1, 1 << 2, false)
	explosion.transform = player.transform
	Global.spawn(explosion);
	player.die();
