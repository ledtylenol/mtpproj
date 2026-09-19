class_name MoveToOtherworld extends Node
@export var particles: Node2D  
@export var update_position: bool  
@export var target: Node2D  
func _ready() -> void:
	particles.physics_interpolation_mode = Node.PHYSICS_INTERPOLATION_MODE_OFF;
	particles.get_parent().call_deferred("remove_child", particles);
	particles.visibility_layer = 1
	Global.call_deferred("spawn_other", particles);

func _physics_process(_delta: float) -> void:
	if (!update_position): return;

	particles.global_position = target.global_position.snapped(Vector2.ONE);
func _exit_tree() -> void:
	if particles:
		particles.queue_free()
