class_name ShakeOnHit extends Node
@export var health: Health 

@export var shaker: ShakerComponent2D

@export var active := true

@export var shake_on_death := true 

func _ready() -> void:
	health.damaged.connect(shake)

func shake(_entity: Entity, _source: HitBox, _damage: float) -> void:
	if (!active): return;
	if (health.current_health == 0 and not shake_on_death): return;
	if (shaker.is_playing): shaker.force_stop_shake();
	shaker.play_shake();
