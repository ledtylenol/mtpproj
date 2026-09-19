extends WarperState
class_name WarperBisque
@export var shaker: ShakerComponent2D
@export var timer: Timer
@export var crack: AudioStreamPlayer
@export var tps_count := 18
@export var hitbox: HitBox
@export var health: Health

var tween: Tween
var tps_left := 3
var timer_duration := 0.3
var hit_timer := 0.0
var hit := false
func on_enter():
	health.damaged.connect(on_damaged)

	hit = false
	hit_timer = 0.0
	timer_duration = 0.2
	tps_left = tps_count
	
	Global.world.bg_handler.tween_time_scale(0.9, 1.)
	warper.change_color(Color.BISQUE)
	warper.spawn_cosmetic_explosion(Color.BISQUE)
	
	var glob_pos := warper.get_global_mouse_position()
	var th := randf_range(0, TAU)
	var v := Vector2(cos(th), sin(th)) * 100
	while not Global.world.play_area.has_point(glob_pos + v):
		th = randf_range(0, TAU)
		v = Vector2(cos(th), sin(th)) * 100

	timer.start(timer_duration)
	warp.teleport(glob_pos + v)
func on_exit():
	health.damaged.disconnect(on_damaged)
func physics_tick(delta: float):
	if tps_left <= 0: 
		if (hit or hit_timer > 0.2):
			transitioned.emit("cooldown")
		else: hit_timer += delta
		return
	if timer.time_left <= 0.0:
		var th := randf_range(-PI / 6.0, PI / 6.0)
		var glob_pos := warper.get_global_mouse_position()
		var off := (glob_pos - warper.global_position).limit_length(30).rotated(th)
		if not Global.world.play_area.has_point(glob_pos) or warper.global_position.distance_to(glob_pos) < 15:
			off = Global.player.global_position - warper.global_position
			tps_left = 1
			hitbox.active = true
		warp.relative(off)
		crack.play()
		timer.start(timer_duration)
		timer_duration -= 0.02
		timer_duration = maxf(timer_duration, 0.035)
		tween_shaker()
		tps_left -= 1

	warper.velocity = warper.velocity.move_toward(Vector2.ZERO, warper.friction * delta)
	warper.move(delta)
func tick(_delta: float):
	Global.world.randomize_label()

func tween_shaker() -> void:
	if tween: tween.kill()
	tween = TweenUtils.default_tween(self)
	tween.tween_property(shaker, "intensity", 0.0, .9).from(15)
func on_damaged(_e, _h, _d) -> void:
	if tps_left > 0:
		hitbox.active = false
	tps_left = 0
	var dir := Global.player.global_position.direction_to(warper.global_position)
	warp.relative(dir * 10)
	hit = true
