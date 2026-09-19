class_name DuplicatorDuplicate extends State
@export var duplicator: Duplicator  

@export var duplicate_timer: Timer  

var time_left: float = 0;
func on_enter() -> void:
	time_left = 1.0;

	duplicate_timer.start(randf_range(2.0, 4.0));

func on_exit() -> void:
	pass

func physics_tick(delta: float) -> void:
	time_left -= delta;
	if (time_left <= 0):
		spawn();
		return;
	var weight = -delta * duplicator.friction;
	duplicator.current_speed = lerpf(duplicator.current_speed, 0, 1.0 - exp(weight));
	duplicator.move(delta);

func tick(_delta: float) -> void:
	pass

func spawn() -> void:
	duplicator.duplicate_spawn();
	transitioned.emit("follow");
