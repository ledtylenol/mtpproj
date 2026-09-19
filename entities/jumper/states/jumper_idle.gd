class_name JumperIdle extends JumperState
@export var jump_timer: Timer  
@export var sprite: Sprite2D  

@export var jump_duration: float  

var tween: Tween 
func on_enter() -> void:
	pass

func on_exit() -> void:
	pass

func physics_tick(delta: float) -> void:
	jumper.velocity = jumper.velocity.move_toward(Vector2.ZERO, delta * jumper.friction);
	jumper.move(delta);
	if (jump_timer.time_left <= 0 && (not tween or not tween.is_running())):
		jump();
		return;

func tick(_delta: float) -> void:
	pass

func jump() -> void:
	if tween: tween.kill()
	tween = TweenUtils.create_tween(self, Tween.EaseType.EASE_OUT, Tween.TransitionType.TRANS_BACK)
	tween.tween_property(sprite, "scale", Vector2(1.2, 0.8), jump_duration).from(Vector2.ONE);
	tween.tween_callback(transitioned.emit.bind("jump"));
	tween.set_trans(Tween.TransitionType.TRANS_ELASTIC);

	tween.tween_property(sprite, "scale", Vector2(0.7, 1.3), jump_duration / 2);
	tween.tween_property(sprite, "scale", Vector2.ONE, jump_duration / 2);
