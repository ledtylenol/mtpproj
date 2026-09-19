class_name HealthDisplay extends HBoxContainer
@export var health_icon_scene: PackedScene

@export var initial_delay: float

@export var delay_per_icon: float

var icons: Array[HealthIcon] = []

var tween: Tween

func _ready() -> void:
	Global.player.health.died.connect(on_player_death);
	for i in Global.player.health.max_health - 1:
		var inst = health_icon_scene.instantiate()
		add_child(inst);
		icons.push_back(inst);
	update_icons();

	Global.player.health.health_changed.connect(on_health_changed)

func on_health_changed(_entity: Entity, _oldHealth: float, _newHealth: float) -> void:
	update_icons();

func update_icons() -> void:
	if tween: tween.custom_step(9999);
	if tween: tween.kill()
	tween = create_tween().set_parallel();
	var delay = initial_delay;
	var health = Global.player.health;
	print(Global.player.health.max_health - 1)
	for i in Global.player.health.max_health - 1:
		var icon = icons[i];
		if i < health.current_health - 1 && !icon.filled:
			tween.tween_subtween(icon.show_icon()).set_delay(delay);
			delay += delay_per_icon;
		if i >= health.current_health - 1 && icon.filled:
			tween.tween_subtween(icon.hide_icon()).set_delay(delay);
			delay += delay_per_icon;
	if (delay == initial_delay): 
		tween.kill();
		print("AH")
	
func on_player_death(_entity: Entity) -> void:
	if tween: tween.kill()
	tween = create_tween().set_parallel();
	var delay = 0.25;
	var health = Global.player.health;
	for i in health.max_health - 1:
		var icon = icons[health.max_health - i - 2]
		tween.tween_subtween(icon.minimize_icon()).set_delay(delay);
		delay += 0.15;
