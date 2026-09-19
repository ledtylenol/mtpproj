class_name JumperJump extends JumperState
@export var Shaker: ShakerComponent2D  

@export var jump: AudioStreamPlayer  
@export var land: AudioStreamPlayer  
@export var jump_cooldown: Timer  

@export var jump_timer: Timer  
@export var ghosts: Ghosts  

@export var max_rand_length: float  
func on_enter() -> void:
	var th = randf_range(0, TAU);
	var r = randf_range(0, max_rand_length);
	var rand_vec = Vector2(cos(th), sin(th)) * r;

	jumper.jump(get_dir() + rand_vec);

	jump.play();
	jumper.tween.finished.connect(transitioned.emit.bind("idle"))

	jump_cooldown.start(jumper.jump_time - 0.1);

	ghosts.active = true;

func on_exit() -> void:
	if Shaker.is_playing: Shaker.force_stop_shake()
	Shaker.play_shake();
	jumper.hit_box.active = false;
	land.play();

	ghosts.active = false;
	if (jumper.cooldown > 0):
		jump_timer.start(jumper.cooldown * randf_range(0.9, 1.1));

func physics_tick(delta: float) -> void:
	jumper.move_bounce(delta);
	jumper.hit_box.active = jump_cooldown.time_left <= 0
	jumper.hurt_box.active = jump_cooldown.time_left <= 0

func tick(_delta: float) -> void:
	pass

func get_dir() -> Vector2:
	var player_pos = Global.player.global_position;
	return (player_pos + Global.player.velocity * jumper.jump_time - jumper.global_position) / jumper.jump_time;
