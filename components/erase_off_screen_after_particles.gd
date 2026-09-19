class_name EraseOffScreenAfterParticles extends EraseOffScreen
@export var particles: GPUParticles2D
func erase() -> void:
	print("ERASE");
	get_tree().create_timer(particles.lifetime).timeout.connect(target.queue_free)
	particles.emitting = false;
