class_name EraseOnDeath extends Node
@export var health: Health

@export var hide_time: float = 0.0;
func _ready() -> void:
	health.died.connect(die)

func die(entity: Entity ) -> void:
	entity.hide();
	if (hide_time > 0):
		get_tree().create_timer(hide_time).timeout.connect(entity.queue_free)
	else: entity.queue_free();
