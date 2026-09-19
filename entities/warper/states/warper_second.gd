extends WarperState
class_name WarperSecond
@export var shaker: ShakerComponent2D
@export var timer: Timer
@export var crack: AudioStreamPlayer
@export var hitbox: HitBox
@export var tps_count := 18
var tween: Tween
var tps_left := 3
var warp_points: Array[Vector2] = []
var timer_duration := 0.3
func on_enter():
	hitbox.active = true
	timer_duration = 0.3
	warp_points.clear()
	for i in tps_count + randi() % 15 - 7:
		var theta := randf_range(0, TAU)
		var off := Vector2(cos(theta), sin(theta)) * randf_range(80, 150)  + Global.player.global_position
		while not Global.world.play_area.has_point(off):
			theta = randf_range(0, TAU)
			off = Vector2(cos(theta), sin(theta)) * randf_range(80, 150) + Global.player.global_position
		warp_points.push_back(off)
	tps_left = tps_count
	Global.world.bg_handler.tween_time_scale(0.9, 1.)
	var th := randf_range(0.0, TAU)
	var v := Vector2(cos(th), sin(th))
	Global.world.bg_handler.tween_dir(v, 0.1)
	warper.change_color(Color.SKY_BLUE)
	warper.spawn_cosmetic_explosion(Color.SKY_BLUE)

func on_exit():
	pass
func physics_tick(delta: float):
	if timer.time_left <= 0.0 and not warp_points.is_empty():
		var off: Vector2 = warp_points.pop_back()
		warp.teleport(off)
		crack.play()
		timer.start(timer_duration)
		timer_duration -= 0.05
		timer_duration = maxf(timer_duration, 0.015)
		tween_shaker()
		if warp_points.is_empty():
			transitioned.emit("cooldown")
			return
	warper.velocity = warper.velocity.move_toward(Vector2.ZERO, warper.friction * delta)
	warper.move(delta)
func tick(_delta: float):
	Global.world.randomize_label()

func tween_shaker() -> void:
	if tween: tween.kill()
	tween = TweenUtils.default_tween(self)
	tween.tween_property(shaker, "intensity", 0.0, .9).from(50.0)
