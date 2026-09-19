extends WarperState
class_name WarperCooldown
@export var timer: Timer
@export var sprite: Sprite2D
@export var hitbox: HitBox
var latest_state := 0
var tween: Tween
var state_updated := false
func on_enter():
	state_updated = false
	timer.start()
	Global.world.bg_handler.tween_time_scale(0.01, 1.0)

	if tween: tween.kill()
	tween = TweenUtils.default_tween(self).set_ease(Tween.EASE_IN_OUT)
	tween.tween_interval(timer.wait_time / 2)
	tween.tween_callback(update_state)
	tween.tween_property(sprite, "scale", Vector2(1.4, 0.6), timer.wait_time / 2)
	tween.tween_callback(initiate)
	tween.set_ease(Tween.EASE_OUT).set_trans(Tween.TRANS_ELASTIC).tween_property(sprite, "scale", Vector2.ONE, 1.5)
	hitbox.active = false
func on_exit():
	Global.boss_phase_changed.emit()
func physics_tick(delta: float):
	warper.velocity = warper.velocity.move_toward(Vector2.ZERO, warper.friction * delta)
	warper.move(delta)
func tick(_delta: float):
	if not state_updated:
		Global.world.randomize_label()
func update_state() -> void:
	latest_state = (latest_state + 1) % 3
	match latest_state:
		0: Global.world.label.text = "SPITE"
		1: Global.world.label.text = "HATRED"
		2: Global.world.label.text = "CURSOR JAIL"
	state_updated = true
func initiate() -> void:
	match latest_state:
		0:
			transitioned.emit("second")
		1:
			transitioned.emit("red")
		2:
			transitioned.emit("bisque")

	return
