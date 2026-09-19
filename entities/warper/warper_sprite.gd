extends Sprite2D
class_name WarperSprite

var time := 0.0:
	set(v):
		set_instance_shader_parameter("time", v)
		time = v

func _process(delta: float) -> void:
	time += delta * Global.world.bg_handler.time_scale
