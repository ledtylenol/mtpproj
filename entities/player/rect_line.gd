class_name RectLine extends Node2D
@export var radius: Vector2:
	set(v):
		radius = v;
		queue_redraw();
func _ready() -> void:
	await World.single.scene_load_ended
	World.single.current_world.redraw.connect(queue_redraw)
	queue_redraw()
func _draw() -> void:
	if not World.single.current_world: return
	draw_rect(Rect2(-radius / 2, radius), World.single.current_world.color, false, 1);
