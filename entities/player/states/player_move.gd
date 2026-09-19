class_name PlayerMove extends PlayerState
@export var Shooter: BulletShooter  
func on_enter() -> void:
	print("Entered move");

func on_exit() -> void:
	pass

func physics_tick(delta: float) -> void:
	if (player.dead):
		transitioned.emit("dead")
		return;

	player.process_inputs();
	if (direction.is_zero_approx()):
		transitioned.emit("idle")
		return;
	if (Input.is_action_just_pressed("dash")):
		transitioned.emit("dash");
		return;

	velocity = velocity.move_toward(direction * player.move_speed, delta * player.acceleration);
	player.move(delta);

func tick(_delta: float) -> void:
	pass
