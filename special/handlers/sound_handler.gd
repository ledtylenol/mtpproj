extends Node
@export var control_fade_audio: AudioStreamPlayer 
@export var shepard_sound: ShepardSound
func on_fade(node: Control) -> void:
	play_pressed(node.get_index(), max(1, node.get_parent().get_child_count() - 1));

func play_pressed(from_index: int, from_max: int) -> void:
	control_fade_audio.pitch_scale = remap(from_index, 0, max(1, from_max), 0.4, 1.8);
	control_fade_audio.pitch_scale = clampf(control_fade_audio.pitch_scale, 0.4, 1.8);
	control_fade_audio.play(0);
func hit() -> void:
	shepard_sound.hit()
