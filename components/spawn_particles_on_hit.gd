class_name SpawnParticlesOnHit extends Node
@export var particles: PackedScene

@export var health: Health

@export var delay := 0.0

func _ready() -> void:
	health.damaged.connect(spawn_particles)

func spawn_particles(entity: Entity, source: HitBox, _damage: float) -> void:
	var dir := source.global_position.direction_to(entity.global_position);
	if (source.owner is Entity): dir = source.owner.velocity.normalized();
	var entity_transform := entity.global_transform;

	if (health.current_health > 0): return;
	if (delay > 0): await get_tree().create_timer(delay, false).timeout

	var inst = particles.instantiate();
	inst.emitting = true;
	inst.one_shot = true;
	inst.transform = entity_transform;
	inst.look_at(inst.position + dir);
	Global.spawn_other(inst);
