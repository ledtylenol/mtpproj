class_name Cursor extends Node2D
@export var player: Player  

@export var sprite: Sprite2D  

@export var width: float  

@export var norm_radius: float  

@export var line_length: float  

@export var dash: float  

@export var color: Color  

@export var shift_speed: float  

@export var shift_power: float  


var mouse_dir: Vector2:
	set(v):
		if v.is_zero_approx():return
		mouse_dir = v;
		queue_redraw();

var mouse_pos: Vector2 

var hue_shift: float 

func _physics_process(delta: float) -> void:
	mouse_pos = get_global_mouse_position();

	mouse_dir = player.position.direction_to(mouse_pos);
	hue_shift += delta * shift_speed;
	sprite.flip_h = mouse_dir.x <= 0;

func _draw() -> void:
	var norm = mouse_dir.normalized() * norm_radius;
	var col = color;
	var hue = (sin(hue_shift) + 1) * 0.5;
	col.ok_hsl_h = hue * shift_power + color.ok_hsl_h;

	draw_dashed_line(norm, norm + mouse_dir.normalized() * line_length, col, width, dash, false);
