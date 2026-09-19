class_name LevelPool extends Resource
@export var possible_enemies: Array[Enemy] = []

@export var minimum_cost: float  

@export var maximum_cost: float  

func enemy_with_cost_below_max(enemy: Enemy, max_cost: float) -> bool:
	return enemy.cost < max_cost
func get_enemy(max_points: float) -> Enemy:
	var enemy: Enemy
	var pos := possible_enemies.filter(enemy_with_cost_below_max.bind(max_points));
	var size = pos.size();
	if (size == 0): return null;
	elif (size == 1): enemy = pos.front();
	else: enemy = pos[randi() % size];
	return enemy;
