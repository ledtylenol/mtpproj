class_name Ghosts extends Node
@export var node_to_copy: Sprite2D 

@export var spawn_interval: float

@export var active := false

@export var spawn_other := false
var real_cooldown := 0.0
var active_time := 0.0
func _process(delta: float) -> void:
	real_cooldown -= delta;
	
	if ((active or active_time > 0) && real_cooldown <= 0):
		spawn_ghost();
		real_cooldown = spawn_interval;
	active_time -= delta
func spawn_ghost() -> void:
	var sprite := Ghost.new(0.5)

	sprite.texture = node_to_copy.texture
	sprite.transform = node_to_copy.global_transform
	sprite.flip_h = node_to_copy.flip_h

	if spawn_other:
		sprite.visibility_layer |= 2;
		sprite.top_level = true;
		Global.spawn_other(sprite);
	else:
		Global.spawn(sprite);
