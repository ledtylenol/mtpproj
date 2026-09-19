extends Node2D
class_name WeaponManager

var entity: Entity:
	get:return owner as Entity
@export var cooldown_items: int:
	set(v):
		cooldown_items = clampi(v, 0, 4);

@export var count_items: int:
	set(v):
		count_items = clampi(v, 0, 2);
@export var range_items: int:
	set(v):
		range_items = clampi(v, 0, 4);

var current_weapon: Weapon
func _ready() -> void:
	child_order_changed.connect(on_child_order_changed)
	refresh_weapon()
func on_child_order_changed() -> void:
	refresh_weapon()
func _process(delta: float) -> void:
	if Input.is_action_pressed("light") and current_weapon:
		current_weapon.shoot(delta)
func refresh_weapon() -> void:
	if get_child_count() > 0:
		current_weapon = get_child(-1)
	else:
		current_weapon = null
func reset() -> void:
	count_items = 0;
	range_items = 0;
	cooldown_items = 0;
