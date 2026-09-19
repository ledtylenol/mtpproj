class_name OtherWorld extends SubViewport
@export var root: Node2D  

@export var camera: Camera2D

@onready var player_camera: PlayerCamera = Global.player.camera
@export var texture: Sprite2D
func _ready() -> void:
	Global.spawn_other_world.connect(spawn_node);
	Global.world.player_died.connect(clear_viewport.unbind(1))
	Global.boss_phase_changed.connect(clear_viewport)
func spawn_node(node: Node2D) -> void:
	root.add_child(node);
func clear_viewport() -> void:
	for child in root.get_children(): if (child is Node2D): child.hide();
	render_target_clear_mode = SubViewport.ClearMode.CLEAR_MODE_ONCE;
