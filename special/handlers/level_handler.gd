extends Node
class_name LevelHandler
@export var pools: Array[LevelPool] = [];

@export var boss_pools: Array[LevelPool] = [];

@export var stats: ExplosionStats 

@export var powerup_scenes: Array[PackedScene] = []

@export var item_spawn_wrapper: PackedScene
var active := false 
var alive_enemies: Array[Entity] = []
var decorative_enemies: Array[Entity] = []
var next_powerup: PackedScene = null;

var current_difficulty := 1.0 
var current_level := 0
static var current: LevelHandler
signal level_finished();

signal level_started();

signal boss_finished();

signal boss_started();

signal boss_next_turn();

signal boss_enemy_killed();

func _ready() -> void:
	current = self
func is_boss() -> bool:
	return (current_level + 1) % 6 == 0;

func will_be_boss() -> bool:
	return (current_level + 2) % 6 == 0;

func is_pool_available(pool: LevelPool) -> bool:
	return pool.minimum_cost <= current_difficulty && pool.maximum_cost >= current_difficulty
func spawn_boss() -> void:
	active = true;

	var pool: LevelPool;
	var possible_pools = boss_pools.filter(is_pool_available);
	var size = possible_pools.size();
	if (size < 1): pool = possible_pools.first();
	else: pool = possible_pools[randi() % size];
	var tween := create_tween().set_parallel();
	var delay := 0.0;
	level_started.emit();
	boss_started.emit();
	var play_area = Global.world.play_area;
	var xRange = play_area.size.x;
	var yRange = play_area.size.y;
	var enemies = pool.possible_enemies;
	current_difficulty += 0.5;
	for enemy in enemies:
		var inst = enemy.scene.instantiate();

		inst.health.died.connect(on_enemy_death);
		var wrapper = enemy.spawn_wrapper.instantiate();
		wrapper.init(inst);

		var subtween = create_tween();
		subtween.tween_callback(Global.spawn.bind(wrapper));
		tween.tween_subtween(subtween).set_delay(delay);

		var pos := Vector2(randf_range(5, xRange - 5), randf_range(5, yRange - 5));

		if (enemy.randomize_position):
			inst.position = pos + play_area.position;
		if (randi() % 3 != 0):
			delay += randf_range(0.05, 0.25);
		alive_enemies.push_back(inst);

	next_powerup = powerup_scenes.pick_random();
	print(powerup_scenes.size());

func spawn_enemies() -> void:
	var total_points = current_difficulty * 15;
	active = true;


	var pool: LevelPool;
	var possible_pools = pools.filter(is_pool_available);
	var size := possible_pools.size();
	if (size < 1): pool = possible_pools.first();
	else: pool = possible_pools[randi() % size];
	var tween := create_tween().set_parallel();
	var delay := 0.0;
	level_started.emit();
	while (total_points > 0.0):

		var play_area = Global.world.play_area;
		var xRange = play_area.size.x;
		var yRange = play_area.size.y;
		var pos := Vector2(randf_range(5, xRange - 5), randf_range(5, yRange - 5));

		var enemy = pool.get_enemy(total_points);
		if (not enemy):
			break;
		var inst = enemy.scene.instantiate();
		if (enemy.randomize_position):
			inst.position = pos + play_area.position;

		if (inst.counts_toward_enemies):
			inst.health.died.connect(on_enemy_death);
		else:
			inst.health.died.connect(on_decorative_enemy_death)
		var wrapper = enemy.spawn_wrapper.instantiate();
		wrapper.init(inst);

		var subtween = create_tween();
		subtween.tween_callback(Global.spawn.bind(wrapper));
		tween.tween_subtween(subtween).set_delay(delay);

		if (randi() % 3 != 0):
			delay += randf_range(0.05, 0.25);
		total_points -= enemy.cost;

		if (inst.counts_toward_enemies):
			alive_enemies.push_back(inst);
		else: decorative_enemies.push_back(inst);

func on_enemy_death(entity: Entity) -> void:

	alive_enemies.erase(entity);
	if (is_boss()): boss_enemy_killed.emit();
	if (alive_enemies.is_empty()):
		if (Global.player.dead): return;
		print("All enemies are dead");
		spawn_explosion()

		if (Global.player.dead): return;

		current_difficulty += 0.1;
		level_finished.emit();
		if (is_boss()):
			boss_finished.emit();

		if (will_be_boss()):
			boss_next_turn.emit();

		current_level += 1;
		active = false;
		if (not next_powerup): return;


		var inst = next_powerup.instantiate() as PowerupContainer;
		inst.position = Vector2.UP * 25;
		# var wrapper = new spawn_wrapper(ItemExplosionStats, inst);
		var wrapper = item_spawn_wrapper.instantiate() as SpawnWrapper
		wrapper.node_to_spawn = inst
		get_tree().create_timer(randf_range(0.1, 0.6)).timeout.connect(Global.spawn.bind(wrapper))
		next_powerup = null;

func on_decorative_enemy_death(entity: Entity) -> void:
	decorative_enemies.erase(entity);

func _physics_process(_delta: float) -> void:
	if (!Global.player.dead && Input.is_action_just_pressed("proceed") && alive_enemies.is_empty()): start_new_level();

func reset() -> void:
	current_difficulty = 1;
	current_level = 0;
	alive_enemies.clear();
	decorative_enemies.clear();
	level_finished.emit();
	next_powerup = null;

func start_new_level() -> void:
	if ((current_level + 1) % 6 == 0): spawn_boss();
	else: spawn_enemies();
func spawn_enemy(enemy: Entity) -> void:
	Global.spawn(enemy);
	alive_enemies.push_back(enemy);
	enemy.health.died.connect(on_enemy_death)
func spawn_explosion(custom_pos := false, pos: Vector2 = Vector2.ONE * 10000) -> void:
	var play_area = Global.world.play_area;
	var expl := Explosion.new(stats, 0, 0, false)
	if not custom_pos:
		expl.position = play_area.position + play_area.size / 2
	else:
		expl.position = pos
	expl.z_index = 100
	Global.spawn(expl);
