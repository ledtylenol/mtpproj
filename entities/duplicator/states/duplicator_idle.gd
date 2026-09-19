class_name DuplicatorIdle extends State
@export var duplicator: Duplicator  
@export var follow_timer: Timer  
func on_enter() -> void:
	follow_timer.start(0.5);

func on_exit() -> void:
	pass

func physics_tick(delta: float) -> void:
	var weight: float = -delta * duplicator.friction;
	duplicator.current_speed = lerpf(duplicator.current_speed, 0, 1.0 - exp(weight));
	duplicator.move(delta);
	if (follow_timer.time_left <= 0):
		transitioned.emit("follow")

func tick(_delta: float) -> void:
	pass
