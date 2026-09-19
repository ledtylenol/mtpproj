class_name SettingsUi extends Control
@export var sfx_slider: HSlider 

@export var sfx_label: Label 

@export var sfx_container: HBoxContainer 

@export var music_slider: HSlider 

@export var music_label: Label 

@export var music_container: HBoxContainer 

var tween: Tween 
func _ready() -> void:
	sfx_slider.value = Global.settings.sfx_volume;
	music_slider.value = Global.settings.music_volume;
	sfx_slider.value_changed.connect(on_sfx_changed)
	music_slider.value_changed.connect(on_music_changed)

	sfx_slider.drag_ended.connect(save_settings)
	music_slider.drag_ended.connect(save_settings)


	on_sfx_changed(sfx_slider.value);
	on_music_changed(music_slider.value);

	PauseManager.paused.connect(tween_menu)
	PauseManager.un_paused.connect(tween_normal)

func on_sfx_changed(value: float) -> void:
	Global.settings.set_sfx(value);
	sfx_label.text = "SFX: %.0f%%" % (value * 100);

func on_music_changed(value: float) -> void:
	Global.settings.set_music(value);
	music_label.text = "MUSIC: %.0f%%" % (value * 100);

func save_settings(value_changed: bool) -> void:
	if (value_changed): Global.settings.save();

func tween_menu() -> void:
	if tween: tween.kill()
	tween = create_tween().set_ease(Tween.EaseType.EASE_IN_OUT).set_trans(Tween.TransitionType.TRANS_EXPO).set_parallel();

	var delay = 1.25;

	var subt = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_ELASTIC);
	subt.tween_property(sfx_container, "offset_transform_position:x", -355, 1.0);

	tween.tween_subtween(subt).set_delay(delay);
	delay += 0.05;

	var subt2 = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_ELASTIC);
	subt2.tween_property(music_container, "offset_transform_position:x", -355, 1.0);

	tween.tween_subtween(subt2).set_delay(delay);

func tween_normal() -> void:
	if tween: tween.kill()
	tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO).set_parallel();

	var delay = 0.0;

	var subt = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO);
	subt.tween_property(sfx_container, "offset_transform_position:x", 0.0, 1.0);

	tween.tween_subtween(subt).set_delay(delay);
	delay += 0.05;

	var subt2 = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO);
	subt2.tween_property(music_container, "offset_transform_position:x", 0.0, 1.0);

	tween.tween_subtween(subt2).set_delay(delay);
