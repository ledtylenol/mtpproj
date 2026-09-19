class_name PlayerDash extends PlayerState
@export var ghosts: Ghosts  
@export var dash_speed := 200.0;

@export var dash_distance := 100.0;

var distance_travelled: float = 0.0;
var start_vel: float = 0;
func on_enter() -> void:
	start_vel = velocity.length();
	velocity = direction * dash_speed;
	distance_travelled = 0;
	ghosts.active = true;

func on_exit() -> void:
	ghosts.active = false;

func physics_tick(delta: float) -> void:
	distance_travelled += (velocity * delta).length();

	if (distance_travelled >= dash_distance):
		if (direction.length() != 0):
			velocity = direction * start_vel;
			transitioned.emit("move");
		else:
			transitioned.emit("idle");
	player.move(delta);

func tick(_delta: float) -> void:
	pass
