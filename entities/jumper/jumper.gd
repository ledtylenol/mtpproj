class_name Jumper extends Entity
@export var sprite: Sprite2D  

@export var jump_height: float  
@export var jump_curve: Curve  

@export var jump_timer: Timer  
@export var cooldown := 0.5;

@export var jump_time := 0.5;
@export var max_jump_distance := 75.0;

@export var hit_box: HitBox  

@export var hurt_box: HurtBox  
@export var x := 0.0;

var tween: Tween 

func _ready() -> void:
	jump_timer.start(randf_range(0.5, 1.8));

func _physics_process(delta: float) -> void:
	super(delta)
	sprite.position.y = -jump_curve.sample_baked(x) * jump_height

func jump(pos: Vector2) -> void:
	pos = pos.limit_length(max_jump_distance);
	print("jumpED");
	if tween: tween.kill()
	hit_box.active = false;

	var s := signf(pos.x);
	tween = TweenUtils.create_tween(self, Tween.EaseType.EASE_OUT, Tween.TransitionType.TRANS_CUBIC)
	tween.tween_property(self, "x", 1.0, jump_time / 2.0);
	tween.parallel().tween_property(sprite, "rotation", s * PI, jump_time / 2.0).from(0.0);
	tween.set_ease(Tween.EaseType.EASE_IN).tween_property(self, "x", 0.0, jump_time / 2.0);
	tween.parallel().tween_property(sprite, "rotation", s * TAU, jump_time / 2.0);
	velocity = pos;

func move_bounce(_delta: float) -> void:
	var iterations = 5;
	var subdelt = _delta / iterations;

	for i in iterations:
		var res = move_and_collide(velocity * subdelt);

		if res:
			velocity = velocity.bounce(res.get_normal());
