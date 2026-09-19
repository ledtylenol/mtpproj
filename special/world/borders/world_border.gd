class_name WorldBorder extends StaticBody2D

@export var separation: float  
@export var world: GameWorld  

@export var left: CollisionShape2D  
@export var right: CollisionShape2D  
@export var up: CollisionShape2D  
@export var down: CollisionShape2D  


func _ready() -> void:
	world.play_area_updated.connect(update_shapes)
	update_shapes(world.play_area);


func update_shapes(new_area: Rect2) -> void:
	print(new_area);
	var dX := -new_area.size.x / 2;
	var dY := -new_area.size.y / 2;

	var vert := Vector2(0, dY - separation);
	var hor := Vector2(dX - separation, 0);

	left.position = -hor;
	right.position = hor;

	up.position = -vert;
	down.position = vert;
