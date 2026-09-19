extends WarperState
class_name WarperRed
@export var shaker: ShakerComponent2D
@export var timer: Timer
@export var crack: AudioStreamPlayer
@export var health: Health
@export var tps_count := 18
@export var hitbox: HitBox
var tween: Tween
var tps_left := 3
var warp_points: Array[float] = []
var next_pos := Vector2.ZERO
var timer_duration := 0.05
var start_pos := Vector2.DOWN
var start_position := Vector2.DOWN
var dirs: Array[Vector2] = [Vector2.RIGHT, Vector2.LEFT, Vector2.UP, Vector2.DOWN]
var former_pos := Vector2.ZERO
var hit := false
var hit_timer := 0.0
func on_enter():
	health.damaged.connect(on_damaged)
	
	hitbox.active = true
	hit = false
	hit_timer = 0.0
	
	warp_points.clear()
	var dir: Vector2 = dirs.pick_random()
	warp.teleport(Global.player.global_position + dir * 100)
	start_position = Global.player.global_position + dir * 100
	
	tween_shaker()
	timer.start(0.1)
	start_pos = dir
	
	for i in tps_count:
		warp_points.push_back(float(i)/ tps_count)
	
	var th := randf_range(0.0, TAU)
	var v := Vector2(cos(th), sin(th))
	Global.world.bg_handler.tween_time_scale(0.9, 1.)
	Global.world.bg_handler.tween_dir(v, 0.1)
	warper.change_color(Color.CRIMSON)
	warper.spawn_cosmetic_explosion(Color.CRIMSON)
func on_exit():
	health.damaged.disconnect(on_damaged)
func physics_tick(delta: float):
	if warp_points.is_empty(): 
		if (hit or hit_timer > 0.2):
			transitioned.emit("cooldown")
		else: hit_timer += delta
		return
	if timer.time_left <= 0.0:
		var off: float = warp_points.pop_back()
		var th := -PI/8 if randi() % 2 == 0 else PI/8
		var pos := former_pos if off == 0.0 else \
		Global.player.global_position + Global.player.velocity * timer_duration
		var dir := warper.global_position.direction_to(pos)
		print(off)
		warp.teleport(pos - dir.rotated(th * off) * off * 100 )
		crack.play()
		timer.start(timer_duration)
		tween_shaker()
		former_pos = Global.player.global_position

func tick(_delta: float):
	Global.world.randomize_label()

func tween_shaker() -> void:
	if tween: tween.kill()
	tween = TweenUtils.default_tween(self)
	tween.tween_property(shaker, "intensity", 0.0, .9).from(50.0)
func on_damaged(_e, _h, _d) -> void:
	warp_points.clear()
	var dir := Global.player.global_position.direction_to(warper.global_position)
	warp.relative(dir * 10)
	hit = true
	hitbox.active = false
