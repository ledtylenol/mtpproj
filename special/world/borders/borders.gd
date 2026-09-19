@tool
class_name Borders extends Node2D
@export var world: GameWorld  

func _enter_tree() -> void:
	if Engine.is_editor_hint(): return
	get_window().maximize_disabled = true;
func _ready() -> void:
	if (Engine.is_editor_hint()): return;
	world.redraw.connect(queue_redraw)
	get_window().size_changed.connect(queue_redraw)
	get_window().maximize_disabled = false;
func _draw() -> void:
	var r := Rect2(-world.size / 2, world.size);
	draw_rect(r, world.color, false, 1);
	var outerRect: Rect2 = get_viewport_rect();
	outerRect.position -= outerRect.size / 2
	outerRect.position += Vector2(0.5, 0.5)
	outerRect.size -= Vector2(0.5, 0.5)
	draw_rect(outerRect, world.color, false, 1);
