class_name Bullet extends Entity
@export var hit_box: HitBox

var root: BulletShooter 
var direction: Vector2 
@warning_ignore("shadowed_global_identifier")
var range: float 

func _ready() -> void:
	#pass the owner to the hit_box for ignore purposes
	hit_box.out_of_hits.connect(queue_free.unbind(1))
	LevelHandler.current.level_finished.connect(handle_level_changed)
	print(range)
func _physics_process(delta: float) -> void:
	move(delta);

func move(delta: float) -> void:

	var wish_vel = direction * move_speed;
	if (acceleration > 0): velocity = velocity.move_toward(wish_vel, delta * acceleration);
	else: velocity = wish_vel;

	move_and_slide();
	range -= (velocity * delta).length();
	if (range <= 0): queue_free();
func handle_level_changed() -> void:
	queue_free();
