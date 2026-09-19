class_name ExplosionStats extends Resource
@export var inner_width_curve: Curve 
@export var outer_width_curve: Curve 
@export var init_ease_type: Tween.EaseType
@export var init_trans_type: Tween.TransitionType
@export var ease_type: Tween.EaseType
@export var trans_type: Tween.TransitionType 
@export var end_radius: float

@export var init_duration: float
@export var duration: float
const active_width_threshold = 0.9;
const mask_all = 1 << 1 | 1 << 2;
const layer_all = 1 << 1 | 1 << 2;

const mask_enemy = 1 << 2;
const layer_enemy = 1 << 1;

const mask_player = 1 << 1;
const layer_player = 1 << 2;

@export var damage: int 

func radius(ratio: float) -> float:
	return lerpf(0, end_radius, ratio);
func inner_width(ratio: float) -> float:
	return inner_width_curve.sample_baked(ratio);
func outer_width(ratio: float) -> float: 
	return outer_width_curve.sample_baked(ratio);
func get_and_start_tween(expl: Explosion) -> Tween:
	var tween = expl.create_tween().set_ease(init_ease_type).set_trans(init_trans_type);

	if (init_duration > 0):
		tween.tween_property(expl, "init_ratio", 1.0, init_duration);
	tween.tween_callback(expl.activate);
	tween.set_ease(ease_type).set_trans(trans_type).tween_property(expl, "ratio", 1.0, duration);

	return tween;
func get_tween(expl: Explosion ) -> Tween:
	return expl.create_tween().set_ease(ease_type).set_trans(trans_type);
