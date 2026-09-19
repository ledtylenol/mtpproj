# utility to access player easier */

@abstract class_name PlayerState extends State
@export var player: Player  


var direction: Vector2:
	get:
		return player.direction
	set(v):
		player.direction = v

var velocity: Vector2:
	get:
		return player.velocity
	set(v):
		player.velocity = v

func _ready() -> void:
	if not player: player = owner as Player
