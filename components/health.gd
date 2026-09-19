class_name Health extends Node
@export var current_health: int:
	set(v):
		
		var old_health := current_health;
		current_health = clampi(v, 0, max_health);
		health_changed.emit(owner as Entity, old_health, current_health);
@export var max_health: int: 
	set(v):
		max_health = v
		current_health = mini(current_health, v);
@export var i_frames: float
@warning_ignore_start("unused_signal")
signal max_health_changed(entity: Entity, old_health: float, new_health: float);

signal health_changed(entity: Entity, old_health: float, new_health: float);

signal damaged(entity: Entity, source: HitBox, damage: float);

signal healed(entity: Entity, source: HitBox, damage: float);

signal died(entity: Entity);

signal already_dead(entity: Entity);

func _ready() -> void:
	current_health = max_health;

func hit(_hurt_box: HurtBox, source: HitBox) -> void:
	if (current_health == 0): already_dead.emit(owner); return; 
	current_health -= source.damage;
	damaged.emit(owner as Entity, source, source.damage);
	if (current_health == 0): died.emit(owner as Entity);
