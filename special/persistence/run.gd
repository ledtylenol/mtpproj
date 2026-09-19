class_name Run extends Resource
@export var time := 0.0;

@export var score := 0.0;

@export	var powerups: Dictionary[String, int] = {}

func add_powerup(powerup_name: String) -> void:
	var count = 1;
	if powerups.has(powerup_name):
		count += powerups[powerup_name];
	powerups[powerup_name] = count;
