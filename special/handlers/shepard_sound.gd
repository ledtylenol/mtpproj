class_name ShepardSound extends AudioStreamPlayer
var heat: float = 0.0;


func hit() -> void:
	heat += 0.1;
	pitch_scale = 1 + log(1 + heat);
	play();

func _process(delta: float) -> void:
	heat = lerpf(heat, 0.0, 1.0 - exp(-delta * 5))
