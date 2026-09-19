@abstract
extends State
class_name WarperState

@export var warp: Warp
var warper: Warper:
	get:
		return owner as Warper
