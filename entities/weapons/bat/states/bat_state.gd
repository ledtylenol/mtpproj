@abstract extends State
class_name BatState

@export var rot_node: Node2D
@export var hitbox: HitBox
@export var sprite: Sprite2D

var bat: BatWeapon:
	get: return owner as BatWeapon
