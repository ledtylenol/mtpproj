extends Camera2D
@export var player: Player

func _physics_process(_delta: float) -> void:
	global_transform = player.camera.global_transform
	zoom = player.camera.zoom
	
