@abstract 
class_name BulletShooter extends Weapon
@export var entity: Entity
@export var bullet_scene: PackedScene
@export var bullet_count: int
@export var spread: float
@export var separation: float
@warning_ignore_start("shadowed_global_identifier")
@warning_ignore_start("shadowed_variable")
@export var range: float

@export var cooldown: float

var real_cooldown := 0.0

signal shot();
func _ready() -> void:
	if not entity: entity = owner as Entity;
	super()
func shoot(_delta: float) -> void:
	if (real_cooldown <= 0):
		real_cooldown = get_cooldown();

		var dir := get_dir();
		var translation := dir.rotated(PI / 2) * separation;
		var spread := get_spread();
		var bullet_count := get_bullet_count();
		for i in bullet_count:
			var instance := bullet_scene.instantiate() as Bullet;
			instance.range = get_range();
			instance.root = self;
			instance.transform = global_transform.translated(translation * i - translation * (bullet_count / 2) + translation / 2);
			instance.direction = dir.rotated(spread * i - spread * (bullet_count / 2) + spread / 2);
			Global.spawn(instance);
		shot.emit();
func _physics_process(delta: float) -> void:
	#TODO use timer node?
	real_cooldown -= delta;

@abstract func get_dir() -> Vector2

@abstract func get_cooldown() -> float

@abstract func get_bullet_count() -> float

@abstract func get_spread() -> float

@abstract func get_range() -> float
