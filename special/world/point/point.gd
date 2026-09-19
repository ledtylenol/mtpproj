class_name Point extends Entity
@export var points: float 
static var last_index: int = 0;

var tween: Tween
func _ready() -> void:
	scale = Vector2.ONE * (1 + points / 500);
	z_index = -1;
	Global.boss_phase_changed.connect(tween_alpha.bind(0.05))
	LevelHandler.current.level_finished.connect(tween_alpha.bind(1.0))
	tween_alpha(0.2)
func _draw() -> void:
	draw_rect(Rect2(Vector2.ZERO, Vector2.ONE * 3), Color.WHITE);
func move(delta: float) -> void:
	position += velocity * delta;
func tween_alpha(val: float) -> void:
	if tween: tween.kill()
	tween = TweenUtils.default_tween(self)
	tween.tween_property(self, "modulate:a", val, 3)
