class_name EraseOnParticleFinish extends Node
func _ready() -> void:
	var parts := get_parent() as GPUParticles2D;
	if (not parts):
		push_error("Parent must be particles");
		queue_free();
	parts.finished.connect(parts.queue_free)
