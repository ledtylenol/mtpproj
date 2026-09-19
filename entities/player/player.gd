class_name Player extends Entity

@export
var shake: ShakeOnHit

@export
var sprite: Sprite2D

@export
var weapon_manager: WeaponManager 

@export var cursor: Cursor
@export var warp: Warp
@export var camera: PlayerCamera
signal died();

var direction: Vector2
var dead := false;

func _ready() -> void:
	Global.player = self;
	health.died.connect(set_dead)
	health.damaged.connect(do_hit_stop)
func process_inputs() -> void:
	direction = Input.get_vector("a", "d", "w", "s");

func _physics_process(delta: float):

	super(delta)

func _process(delta: float) -> void:

	super(delta)
	state_machine.tick(delta);
	#if (Input.is_action_just_pressed("dash")):
		#warp.blink(get_global_mouse_position())

func set_dead(_entity) -> void:
	dead = true;
	shake.active = false;

func un_die() -> void: 
	dead = false;
	shake.active = true;
	visible = true;
	velocity = Vector2.ZERO;
	position = Vector2.ZERO;
	weapon_manager.reset()
	reset_physics_interpolation()
func die() -> void:
	died.emit()

func do_hit_stop(_e: Entity, _culprit: HitBox, damage: float):
	if (health.current_health == 0): return;
	TimeHandler.stop(0.5, damage / 10.0);
