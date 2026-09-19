extends Weapon
class_name BatWeapon

var state := 0
var tween: Tween
@export var sod: SecondOrderDynamics
@export var point_sod: SecondOrderDynamics
@export var sprite_sod: SecondOrderDynamics
@export var self_sod: SecondOrderDynamics
@export var sprite_pos_sod: SecondOrderDynamics
@export var rot_node: Node2D
@export var rot_point_node: Node2D
@export var sprite: Sprite2D
@export var hitbox: HitBox
@export var vel_rot: Node2D
@export var ghosts: Ghosts
@export var hit_ghosts: Ghosts
@export var charge_shaker: ShakerComponent2D
var charge := 0.0
var target_point_rot := 0.0
var target_sprite_rot := 0.0
var target_sprite_x := 20.0
var timer :=  0.0
var delay  := 0.0
var player_dist := 0.0
var func_tween: Tween
var queue := 0
var own: Entity

func _ready() -> void:
	hitbox.hit_succeeded.connect(on_hit_succeeded)
	super()
	set_deferred("own", weapon_manager.entity)
func shoot(_delta: float) -> void:
	pass


func _process(_delta: float) -> void:
	var dist := Global.player.global_position.distance_to(get_global_mouse_position())
	player_dist = clampf(dist - 10, 20, 20 + weapon_manager.range_items * 20)
	super(_delta)
	var dir := global_position.direction_to(get_global_mouse_position()).angle()
	rotation = self_sod.update_f(rotation, dir, _delta, true)
	vel_rot.rotation = sod.update_f(vel_rot.rotation, 0.0, _delta, true)
	rot_point_node.rotation = point_sod.update_f(rot_point_node.rotation, target_point_rot, _delta, true)
	sprite.rotation = sprite_sod.update_f(sprite.rotation, target_sprite_rot - PI /2 , _delta, true)
	sprite.position.x = sprite_pos_sod.update_f(sprite.position.x, target_sprite_x, _delta, false)
	hitbox.active = timer > 0 && delay <= 0
	hit_ghosts.active = hitbox.active
	timer -= _delta
	delay -= _delta
	hitbox.position.x = player_dist
func _physics_process(_delta: float) -> void:
	call_deferred("clear_queue")
func swing() -> void:
	charge = 0
	if tween:
		tween.kill()
	if state == 0:
		state = 1
	else:
		state = 0

func clear_queue() -> void:
	if queue == 0: return
	if func_tween:
		func_tween.custom_step(90999)
		func_tween.kill()
	func_tween = create_tween().set_ignore_time_scale(true)
	for arr in queue:
		var subt := create_tween().set_ignore_time_scale(true)
		subt.tween_callback(push)
		subt.tween_interval(0.15)
		func_tween.tween_subtween(subt)
	queue = 0
func on_hit_succeeded(_hb: HitBox, hurt_box: HurtBox) -> void:
	var entity := hurt_box.owner as Entity
	if entity:
		entity.velocity += sprite.global_position.direction_to(entity.global_position) * 100
	own.velocity += entity.global_position.direction_to(own.global_position) * 200
	queue += 1
func push() -> void:
	TimeHandler.stop(.05, 0.1)
