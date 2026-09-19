extends Entity

class_name Weapon
var weapon_manager: WeaponManager

func _ready() -> void:
	weapon_manager = get_parent() as WeaponManager
	if not weapon_manager: push_error("parent is not weapon manager")
func shoot(_delta: float) -> void:
	pass
