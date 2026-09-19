class_name HealthIcon extends MarginContainer

@export var shaker: ShakerComponent

@export var filled := true

@export var icon: TextureRect  

@export var shown_texture: Texture2D  

@export var hide_texture: Texture2D 

var tween: Tween 

func hide_icon() -> Tween:
	if tween: tween.kill()
	tween = create_tween();
	tween.tween_callback(func() -> void: icon.texture = hide_texture)
	tween.tween_callback(func() -> void: filled = false)
	tween.tween_callback(func() -> void: shaker.play_shake())
	return tween

func show_icon() -> Tween:
	if tween: tween.kill()
	tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO);
	tween.tween_callback(func() -> void:icon.texture = shown_texture);
	tween.tween_property(icon, "offset_transform_scale", Vector2.ONE, 1).from(Vector2.ZERO);
	tween.tween_callback(func() -> void: filled = true)
	return tween
func minimize_icon() -> Tween:
	if tween: tween.kill()
	tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO);
	tween.tween_property(icon, "offset_transform_scale", Vector2.ZERO, 1).from(Vector2.ONE);
	tween.tween_callback(func() -> void: filled = false)
	return tween
