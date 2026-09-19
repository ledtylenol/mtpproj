class_name PlayerBulletShooter extends BulletShooter
@export var cursor: Cursor  

var cooldown_items: int:
	get:
		return weapon_manager.cooldown_items
var count_items: int:
	get:
		return weapon_manager.count_items
var range_items: int:
	get:
		return weapon_manager.range_items
func get_bullet_count() -> float:
	return bullet_count + count_items;

func get_cooldown() -> float:
	return cooldown - cooldown_items / 50.0;

func get_dir() -> Vector2:
	return cursor.mouse_dir;

func get_spread() -> float:
	return spread;
func get_range() -> float:
	return range + range_items * 50;
