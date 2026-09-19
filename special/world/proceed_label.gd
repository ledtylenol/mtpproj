class_name ProceedLabel extends RichTextLabel

@export var spacing: float:
	set(v):
		spacing = v
		text = "[font gl=%f][font_size=40][center]%s" % [spacing, message]

var message: String

@export var initial_spacing: float 
@export var level_handler: LevelHandler
var tween: Tween 
func _ready() -> void:
	level_handler.level_started.connect(hide)
	level_handler.level_finished.connect(show_finished)
	Global.player.died.connect(show_dead)

func show_finished() -> void:
	tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO)
	tween.tween_property(self, "spacing", 0.1, 2).from(initial_spacing);
	show();
	message = "[TAB]\nPROCEED";

func show_dead() -> void:
	if tween: tween.kill()
	tween = create_tween().set_ease(Tween.EaseType.EASE_OUT).set_trans(Tween.TransitionType.TRANS_EXPO)
	tween.tween_property(self, "spacing", 0.1, 2).from(initial_spacing);
	show();
	message = "YOU DIED\n[TAB]\nPROCEED";
