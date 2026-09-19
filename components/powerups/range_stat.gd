class_name RangeStat extends PowerupStat
func apply(player: Player) -> void:
	player.weapon_manager.range_items += 1;
func can_apply(player: Player) -> bool:
	return player.weapon_manager.range_items < 4;

func get_message() -> String:
	return "Range up";
