class_name Points extends Node
@export var point_count: int 
@export var health: Health 
@export var point_scene: PackedScene 

func _ready() -> void:
	if health:
		health.damaged.connect(spawn_points)

func spawn_points(entity: Entity, source: HitBox, damage: float) -> void:
	if (Global.player.dead): return;
	var ratio = damage / health.max_health;
	var count = 1;
	var ratio_per = ratio / count;

	var th = randf_range(-PI / 4, PI / 4);
	var dir: Vector2
	if source.entity:
		dir = source.entity.velocity.normalized()
	else:
		dir = source.global_position.direction_to(entity.global_position)
	dir = dir.rotated(th) * (120.0 + randf_range(-10, 10));

	var points = ratio_per * point_count;
	for i in count:

		var inst = point_scene.instantiate();
		inst.velocity = dir;
		inst.position = entity.global_position;
		inst.points = points;

		Global.spawn(inst);
func spawn_points_random(source: Node2D) -> void:
	if (Global.player.dead): return;
	var ratio = 1.0;
	var count = 20 + randi() % 10;
	var ratio_per = ratio / float(count);


	var points = ratio_per * point_count;
	for i in count:

		var th = randf_range(0, TAU);
		var dir = Vector2.RIGHT * (120.0 + randf_range(-10, 10));
		dir = dir.rotated(th);
		var inst = point_scene.instantiate();
		inst.velocity = dir;
		inst.position = source.global_position;
		inst.points = points;

		Global.spawn(inst);
