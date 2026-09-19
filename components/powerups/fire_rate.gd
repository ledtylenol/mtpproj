class_name FireRate extends PowerupStat
func apply(player: Player) -> void:
	player.weapon_manager.cooldown_items += 1;
func can_apply(player: Player) -> bool:
	return player.weapon_manager.cooldown_items < 4;

func get_message() -> String:
	return "Firerate up";
