extends WarperState
class_name WarperFirst
@export var shaker: ShakerComponent2D
@export var timer: Timer
@export var crack: AudioStreamPlayer
@export var health: Health
@export var health_to_copy: Health
@export var music: AudioStreamPlayer
@export var hurtbox: HurtBox
@export var teleport_intensity_curve: Curve
var tween: Tween
var initiating := false
var tp_pos := Vector2.ZERO
func on_enter():
	timer.start()
	health.damaged.connect(on_health_damaged)
	Global.world.bg_handler.tween_time_scale(0.1, 5.0)
func on_exit():
	hurtbox.active = true
	Global.boss_phase_changed.emit()
	warp.teleport(tp_pos)
	music.play(10.5)
func physics_tick(delta: float):
	if initiating: return
	if timer.time_left <= 0.0:
		var th := randf_range(0, TAU)
		var off := Vector2(cos(th), sin(th)) * randf_range(30, 90)
		warp.teleport(Global.player.global_position + off)
		crack.play()
		timer.start()
		tween_shaker()
	else:
		shaker.intensity = teleport_intensity_curve.sample_baked(1.0 - timer.time_left / timer.wait_time)
	if !music.playing and health.current_health <= 100:
		music.play()
	
	if music.get_playback_position() >= 10.5:
		music.stop()
		if tween: tween.kill()
		tween = create_tween()
		tween.tween_callback(initiate)
		tween.tween_interval(2)
		tween.tween_callback(transitioned.emit.bind("second"))
	warper.velocity = warper.velocity.move_toward(Vector2.ZERO, warper.friction * delta)
	warper.move(delta)
func tick(_delta: float):
	pass

func tween_shaker() -> void:
	if tween: tween.kill()
	tween = TweenUtils.default_tween(self)
	tween.tween_property(shaker, "intensity", 0.0, 0.5).from(50.0)

func on_health_damaged(_entity: Entity, _source: HitBox, _damage: float) -> void:
	if health.current_health <= 10:
		if initiating: return
		music.stop()
		if tween: tween.kill()
		tween = create_tween()
		tween.tween_callback(initiate)
		tween.tween_interval(2)
		tween.tween_callback(transitioned.emit.bind("second"))
func initiate() -> void:
	tp_pos = warper.global_position
	warp.teleport(Vector2.UP * 500)
	warper.latest_color = Color.SKY_BLUE
	crack.play()
	health.max_health = health_to_copy.max_health
	health.current_health = health_to_copy.current_health
	Global.world.bg_handler.tween_time_scale(0.0, 1.0)
	hurtbox.active = false
	initiating = true
	
