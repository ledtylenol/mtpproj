extends BatState
class_name BatStateSwing
@export var sprite_rot := -PI
@export var point_rot := -PI
@export var sod: SecondOrderDynamics
@export var sprite_pos_sod: SecondOrderDynamics
@export var ding: AudioStreamPlayer2D
@export var swing: AudioStreamPlayer2D
@export var drone: AudioStreamPlayer2D
@export var next_state: BatState
@export var swing_impulse := TAU * 5
@export var charge_ding_impulse_point := -PI * 10
@export var charge_ding_impulse_sprite := -PI * 6
@export var charge_time := 0.5
var charge := 0.0:
	set(v):

		charge = clampf(v, 0, 1)
var start_charge := 0.0:
	set(v):
		start_charge = clampf(v, 0, 1)
func on_enter():
	start_charge += 0.3
	charge = start_charge
func on_exit():
	pass

func tick(_delta: float):
	start_charge -= _delta
	next_state.start_charge -= _delta
	if Input.is_action_pressed("heavy"):
		bat.target_point_rot = point_rot
		bat.target_sprite_rot = sprite_rot
		charge += _delta
		if (charge > charge_time and not drone.playing):
			bat.point_sod.ydf += charge_ding_impulse_point
			bat.sprite_sod.ydf += charge_ding_impulse_sprite
			bat.ghosts.active = true
			bat.charge_shaker.play_shake()
			ding.play()
			drone.play()
		bat.sprite_sod._f = 1
		bat.target_sprite_x = bat.player_dist +  10
	else:
		bat.target_point_rot = 0
		bat.target_sprite_rot = 0
		charge -= _delta * 3
		bat.sprite_sod._f = 2
		bat.target_sprite_x = 20

	if Input.is_action_just_released("heavy") and charge >= charge_time:
		sod.ydf = swing_impulse
		print("SHOOT")
		charge = 0
		bat.charge_shaker.force_stop_shake()
		bat.ghosts.active = false
		swing.play()
		drone.stop()
		transitioned.emit(next_state.name)
		bat.timer = 0.05
		bat.delay = 0.015
func physics_tick(_delta: float):
	pass
