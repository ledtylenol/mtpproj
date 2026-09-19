extends ColorRect
class_name FaderRect

@export var camera: Camera2D
@export  var alpha := 1.0:
	set(v):
		alpha = v
		set_instance_shader_parameter("alpha", v)
@export  var vert_thres := 1.0:
	set(v):
		vert_thres = v
		set_instance_shader_parameter("vert_thres", v)
var time := 0.0:
	set(v):
		time = v
		set_instance_shader_parameter("time", v)
func _process(delta: float) -> void:
	time += delta

func _physics_process(_delta: float) -> void:
	global_position = camera.global_position - size / 2
