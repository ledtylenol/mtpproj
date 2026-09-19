class_name LeaderboardEntry extends MarginContainer

@export var color_rects: Dictionary[String, Array] 

@export var time_label: Label  

@export var score_label: Label  

@export var tween_duration: float = 2.0
var do_tween := false
var active := true 
var tween: Tween 
@export var notifier: VisibleOnScreenNotifier2D  

var container: SmoothScrollContainer
func _ready() -> void:
	notifier.screen_entered.connect(tween_visible)
	for arr in color_rects.values():
		for p in arr:
			var rect: ColorRect = get_node(p)
			rect.hide();

func initialize(run: Run):
	for key in run.powerups.keys():
		var n = run.powerups[key];
		var current_rects = color_rects[key];
		for i in n:
			var rect = get_node(current_rects[i])
			rect.show();
	time_label.text = "%.0fs" % run.time;
	score_label.text = "%.0fp" % run.score;

func get_init_tween() -> Tween:
	var _tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_ELASTIC)

	_tween.tween_callback(func() -> void:SoundHandler.on_fade(self))
	_tween.tween_property(self, "offset_transform_position:x", 0.0, tween_duration).from(-100);
	_tween.tween_callback(func() -> void:do_tween = true)
	_tween.tween_callback(update_size)
	return _tween;

func update_size() -> void:
	notifier.rect.size = size / 2
	notifier.rect.position = size / 4

func tween_visible() -> void:
	if container.velocity < 5:
		return
	if (!do_tween): return;
	if (!active):
		active = true;
		return;
	if tween: tween.kill()
	tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_ELASTIC)
	tween.tween_callback(func() -> void: SoundHandler.on_fade(self))

	tween.tween_property(self, "offset_transform_position:x", 0.0, tween_duration).from(-100);
