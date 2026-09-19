class_name Leaderboard extends MarginContainer
@export var container: VBoxContainer  
@export var smooth_container: SmoothScrollContainer
@export var leaderboard_entry: PackedScene  
@export var initial_delay: float  
@export var delay: float  

var tween: Tween 
func _ready() -> void:
	Global.run_ended.connect(add_run)
	var runs := Global.runs.runs.duplicate();
	runs.reverse();

	tween = create_tween().set_parallel();

	var i_delay := initial_delay;
	for run in runs:
		var entry: LeaderboardEntry = leaderboard_entry.instantiate();

		entry.container = smooth_container
		container.add_child(entry);
		entry.initialize(run);
		tween.tween_subtween(entry.get_init_tween()).set_delay(i_delay);
		i_delay += delay;
func add_run(run: Run) -> void:
	var entry = leaderboard_entry.instantiate();
	entry.container = smooth_container

	container.add_child(entry);
	container.move_child(entry, 0);
	entry.initialize(run);
	var _tween = create_tween();
	_tween.tween_interval(0.25);
	_tween.tween_subtween(entry.get_init_tween());
