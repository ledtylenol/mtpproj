class_name DuplicatorFollow extends State
@export var duplicator: Duplicator  
@export var player_detection: Area2D  
@export var dash_timer: Timer  

@export var duplicate_timer: Timer  

func on_enter() -> void:
	pass

func on_exit() -> void:
	pass

func physics_tick(delta: float) -> void:
	var weight = -delta * duplicator.turn_speed;

	duplicator.update_dir();
	duplicator.current_speed = lerp(duplicator.current_speed, duplicator.move_speed, 1.0 - exp(weight));
	duplicator.direction = duplicator.direction.slerp(duplicator.player_dir, 1.0 - exp(weight));
	duplicator.move(delta);
	var bodies = player_detection.get_overlapping_bodies();
	if (bodies.size() > 0 && dash_timer.time_left <= 0):
		transitioned.emit("Enraged");
	if (duplicate_timer.time_left <= 0):
		transitioned.emit("Duplicate");

func tick(_delta: float) -> void:
	pass
