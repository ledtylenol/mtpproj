class_name Count extends PowerupStat
func apply(player: Player) -> void:
	player.weapon_manager.count_items += 1;
func can_apply(player: Player) -> bool:
	return player.weapon_manager.count_items < 2;

func get_message() -> String:
	return "Bullet count up";
