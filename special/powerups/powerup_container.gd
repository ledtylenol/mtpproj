class_name PowerupContainer extends Node2D

@export var powerup: Powerup  

@export var sprite: Sprite2D  
@export var sound: AudioStreamPlayer  

func _ready() -> void:
	powerup.powerup_applied.connect(spawn_visuals)
	powerup.active = false;
	powerup.activate_after(0.5);

func spawn_visuals(message: String) -> void:
	sprite.hide();
	var label = Label.new()

	label.text = message
	label.position = global_position - label.size / 2
	label.horizontal_alignment = HorizontalAlignment.HORIZONTAL_ALIGNMENT_CENTER
	label.vertical_alignment = VerticalAlignment.VERTICAL_ALIGNMENT_CENTER

	Global.force_spawn_2d(label);

	var tween = TweenUtils.default_tween(label);
	var th = randf_range(-PI / 2, PI / 2);
	var r = randf_range(20, 80);

	var dir = Vector2.UP.rotated(th) * r;

	var copy = sound.duplicate() as AudioStreamPlayer;
	Global.spawn(copy);
	copy.play();

	copy.finished.connect(copy.queue_free)

	tween.tween_property(label, "position", dir, 1).as_relative();
	tween.tween_callback(label.queue_free);
	queue_free();
