class_name DuplicatorEnraged extends State
@export var duplicator: Duplicator  

@export var ghosts: Ghosts  

@export var sprite: Sprite2D  

@export var shaker: ShakerComponent2D  

@export var hit_box: HitBox  
@export var windup: AudioStreamPlayer  

@export var dash: AudioStreamPlayer  
var tween: Tween 
func on_enter() -> void:
	if tween: tween.kill()
	tween = TweenUtils.default_tween(self)
	tween.tween_callback(windup.play);
	tween.tween_property(sprite, "scale", Vector2(1.2, 0.8), 0.5);
	tween.parallel().tween_property(shaker, "intensity", 1, 0.5);
	tween.parallel().tween_property(duplicator, "current_speed", 0, 0.5);
	tween.tween_callback(func() -> void:
		dash.play();
		duplicator.current_speed = 400;
		duplicator.direction = get_dir();
		ghosts.active = true;
		sprite.scale = Vector2.ONE;
		hit_box.active = true;
	);
	tween.tween_property(shaker, "intensity", 0, 0.5);
	tween.tween_callback(func() -> void:
		transitioned.emit("idle");
		ghosts.active = false;
		hit_box.active = false;
	);
func on_exit() -> void:
	ghosts.active = false;

func get_dir() -> Vector2:

	var player_pos = Global.player.global_position;
	var dist = duplicator.global_position.distance_to(player_pos) / duplicator.current_speed;
	return duplicator.global_position.direction_to(player_pos + Global.player.velocity * dist);

func physics_tick(delta: float) -> void:
	var weight = -delta * duplicator.turn_speed / 5;
	duplicator.update_dir();
	duplicator.direction = duplicator.direction.slerp(duplicator.player_dir, 1.0 - exp(weight));
	duplicator.move(delta);

func tick(_delta: float) -> void:
	pass
