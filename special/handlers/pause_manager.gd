extends Node

signal paused();
signal un_paused()
func _ready() -> void:
	process_mode = PROCESS_MODE_ALWAYS
func toggle_pause() -> void:
	var was_paused = get_tree().paused;
	get_tree().paused = !get_tree().paused;
	if (!was_paused):
		paused.emit();
	else:
		un_paused.emit();
func _physics_process(_delta: float) -> void:
	if (Input.is_action_just_pressed("pause")): toggle_pause();

func pause() -> void:
	get_tree().paused = true;
	paused.emit();

func resume() -> void:
	get_tree().paused = false;
	un_paused.emit();
