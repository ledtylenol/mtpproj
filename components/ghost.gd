class_name Ghost extends Sprite2D
var duration: float
var quant:int
var alpha := 1.0
var delay := 0.0
func _init(_duration: float, _quant: int = 100, _delay := 0.0) -> void:
	duration = _duration
	quant = _quant
	delay = _delay
func _ready() -> void:
	z_index = 1;
	var tween := create_tween();
	tween.tween_interval(delay)
	tween.tween_property(self, "alpha", 0.0, duration);
	tween.tween_callback(queue_free);

func _process(_delta: float) -> void:
	modulate.a = floor(alpha * quant) / quant
