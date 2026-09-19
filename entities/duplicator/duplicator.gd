class_name Duplicator extends Entity
@export var duplicate_sound: AudioStreamPlayer  
@export var sprite: Sprite2D  
@export var turn_speed: float  = 2.5;
@export_file var duplicate_string: String  

static var count: int = 0;
var player_dir: Vector2 
var direction: Vector2 
var current_speed: float 

func _enter_tree() -> void:
	count += 1;
func _ready() -> void:
	sprite.flip_h = randi() % 2 == 0;

func _exit_tree() -> void:
	count -= 1;

func move(_delta: float) -> void:
	velocity = direction * current_speed;
	sprite.rotation = direction.x * PI / 6

	var iterations = 5;
	var subdelt = _delta / iterations;

	for i in iterations:
		var res = move_and_collide(velocity * subdelt);

		if res:
			velocity = velocity.bounce(res.get_normal());
			direction = direction.bounce(res.get_normal());

func update_dir() -> void:
	player_dir = global_position.direction_to(Global.player.global_position);

func duplicate_spawn() -> void:
	if (count > 10): return;
	if (health.current_health < 2): return;
	if (Global.player.dead): return;
	var scene = load(duplicate_string);
	var dup = scene.instantiate() as Duplicator;
	dup.transform = global_transform;
	LevelHandler.current.spawn_enemy(dup);
	dup.current_speed = 50;
	dup.direction = Vector2.RIGHT.rotated(randf_range(0, TAU));
	dup.health.current_health = health.current_health - 1;
	health.current_health -= 1;
	duplicate_sound.play();
