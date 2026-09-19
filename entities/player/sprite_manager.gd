extends Node
class_name SpriteManager

@onready var sprite: Sprite2D = get_parent()
@export var shake: ShakerComponent2D
@export var rect: FaderRect

@export var height := 30.0
@export var spin_speed := -PI/2
@export var start_duration := 0.5
@export var end_duration := 0.5
@export var shake_delay := 0.3
@export var start_ease_type: Tween.EaseType
@export var start_trans_type: Tween.TransitionType

@export var end_ease_type: Tween.EaseType
@export var end_trans_type: Tween.TransitionType


var real_spin_speed := 0.0
var real_height := 0.0

var spin_tween: Tween
func _ready() -> void:
	if not sprite:
		push_error("parent is not sprite") 
	World.single.scene_load_started.connect(start_transition)
	World.single.scene_load_ended.connect(end_transition)
func _physics_process(delta: float) -> void:
	sprite.rotate(real_spin_speed * delta)
	sprite.rotation = wrapf(sprite.rotation, -PI, PI)

func start_transition() -> void:
	if spin_tween: 
		spin_tween.custom_step(9999)
		spin_tween.kill()
	
	spin_tween = TweenUtils.create_tween(self, start_ease_type, start_trans_type, true)
	spin_tween.tween_property(sprite, "position:y", -height, start_duration)
	spin_tween.tween_property(self, "real_spin_speed", spin_speed, start_duration)
	spin_tween.tween_property(rect, "alpha", 1.0, start_duration)
	spin_tween.tween_property(rect, "vert_thres", 1.0, start_duration)

func end_transition() -> void:
	if spin_tween: 
		spin_tween.custom_step(9999)
		spin_tween.kill()
	spin_tween = create_tween().set_parallel(true)
	var subt1 = TweenUtils.create_tween(self, end_ease_type, end_trans_type, true)
	subt1.tween_property(sprite, "position:y", 0, end_duration)
	subt1.tween_property(self, "real_spin_speed", 0, end_duration)
	subt1.tween_property(sprite, "rotation", -TAU, end_duration)
	var subt2 = TweenUtils.create_tween(self, start_ease_type, start_trans_type, true)
	subt2.tween_property(rect, "alpha", 0.0, end_duration)
	subt2.tween_property(rect, "vert_thres", 0.0, end_duration)
	subt2.tween_callback(shake.play_shake)
	spin_tween.tween_subtween(subt1)
	spin_tween.tween_subtween(subt2).set_delay(shake_delay)
