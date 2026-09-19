class_name TweenUtils

static func create_tween(node: Node, ease_type: Tween.EaseType, transitionType: Tween.TransitionType, parallel: bool = false) -> Tween:
	return node.create_tween().set_ease(ease_type).set_trans(transitionType).set_parallel(parallel);

static func default_tween(node: Node, parallel: bool = false) -> Tween:
	return create_tween(node, Tween.EaseType.EASE_OUT, Tween.TransitionType.TRANS_EXPO, parallel)
