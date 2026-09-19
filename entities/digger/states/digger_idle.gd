class_name DiggerIdle extends DiggerState

func on_enter() -> void:
	pass

func on_exit() -> void:
	pass

func physics_tick(delta: float) -> void:
	digger.velocity = digger.global_position.direction_to(Global.player.global_position) * 30;
	digger.move(delta);

func tick(delta: float) -> void:
	pass
