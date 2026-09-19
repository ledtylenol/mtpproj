class_name SmoothScrollContainer extends FoldableContainer
@export var container: ScrollContainer  
@export var duration: float  

@export var scroll_amount: int  

@export var entry_parent: VBoxContainer  
var velocity: float 
var latest_sign: float 
func _ready() -> void:
	folding_changed.connect(on_folding_changed)
func _gui_input(input_event: InputEvent) -> void:
	if (input_event is InputEventMouseButton):
		if (!input_event.pressed): return;

		match input_event.button_index:
			MouseButton.MOUSE_BUTTON_WHEEL_UP: tween_scroll(-scroll_amount)
			MouseButton.MOUSE_BUTTON_WHEEL_DOWN: tween_scroll(scroll_amount)
func _process(delta: float) -> void:
	velocity = lerpf(velocity, 0, 1 - exp(-delta * 5));
	container.scroll_vertical += int(velocity * delta);

func tween_scroll(amount: int) -> void:
	var i_sign := signi(amount);
	if (signi(amount) != latest_sign):
		velocity = amount;
	else:
		velocity += amount;

	latest_sign = i_sign;
func on_folding_changed(_is_folded: bool) -> void:
	for child in entry_parent.get_children():
		var timer = get_tree().create_timer(0.01);
		if (child is LeaderboardEntry):
			timer.timeout.connect(func(): child.active = not is_folded)
