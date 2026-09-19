class_name ParticleDirection extends Node
@export var particles: GPUParticles2D  
@export var entity: Entity  

@export var move_to_draw_world: bool  

@export var update_dir: bool  

func _ready() -> void:
	particles.physics_interpolation_mode = Node.PHYSICS_INTERPOLATION_MODE_OFF;
	particles.top_level = true;
	particles.position = entity.global_position.snapped(Vector2.ONE);

	if update_dir:
		update_direction();
	particles.emitting = true;

func _physics_process(_delta: float) -> void:
	if not update_dir: return;
	if not particles:  queue_free(); return; 
	update_direction();
func update_direction() -> void:
	particles.look_at(particles.global_position - entity.velocity);
	particles.position = entity.global_position.snapped(Vector2.ONE);
func _exit_tree() -> void:
	if not move_to_draw_world: return;
	if particles: particles.queue_free()
