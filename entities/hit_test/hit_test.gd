class_name HitTest extends Entity
@export var explosion_stats: ExplosionStats  
func _ready() -> void:
	health.died.connect(die)

func die(_entity: Entity) -> void:
	if (not Global.player.dead && LevelHandler.current.active):
		var expl = Explosion.new(explosion_stats, explosion_stats.mask_all, explosion_stats.layer_all, true);
		expl.position = Global.player.global_position;

		Global.spawn(expl);
	queue_free();
