class_name PlayerIdle extends PlayerState
func on_enter() -> void:
	pass

func on_exit() -> void:
	pass

func physics_tick(delta: float) -> void:
	if (player.dead):
		transitioned.emit("dead")
		return;
	player.process_inputs();
	if (!direction.is_zero_approx()):
		transitioned.emit("move");
		return;


	velocity = velocity.move_toward(Vector2.ZERO, delta * player.friction);
	player.move(delta);

func tick(_delta: float) -> void:
	pass
