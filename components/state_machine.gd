class_name StateMachine extends Node
@export var initial_state: State
@export var current_state: State 
var states: Dictionary[String, State] = {}

func _ready() -> void:
	if (not initial_state): queue_free(); return;
	# manually control flow of state machine instead of relying on the scene tree
	set_process(false);
	set_physics_process(false);
	for child in get_children():
		var state := child as State
		if (state):
			states[state.name.to_lower()] = state;
			state.transitioned.connect(transition);
	current_state = initial_state;
	initial_state.on_enter();

func tick(delta: float) -> void:
	current_state.tick(delta);

func physics_tick(delta: float) -> void:
	current_state.physics_tick(delta);

#handle transition logic from one state to another
func transition(to: String) -> void:
	var lower = to.to_lower();

	if (!states.has(lower)):
		push_error("could not find %s" % lower);
		return;
	var new_state = states[lower];

	current_state.on_exit();

	current_state = new_state;

	current_state.on_enter();
