class_name Powerup extends Area2D

@export var particles: PackedScene  

@export var stat: PowerupStat  

@export var powerup_name: String  

@export var active: bool  = true

@export var points: Points  

signal powerup_applied(message: String)

func _ready() -> void:
	body_entered.connect(on_body_entered)

func on_body_entered(body: Node2D) -> void:
	if (!active): return;
	var player: Player = body as Player
	if not player: return;

	var could_apply := stat.can_apply(player);
	if not could_apply:
		points.spawn_points_random(self);
		active = false;
		spawn_particles();
		powerup_applied.emit("+500p");
		return;

	stat.apply(player);
	powerup_applied.emit(stat.get_message());
	Global.run.add_powerup(powerup_name);
	active = false;
func activate_after(time: float) -> void:
	get_tree().create_timer(time).timeout.connect(set.bind("active", true))

func spawn_particles() -> void:
	var inst = particles.instantiate();
	inst.emitting = true;
	inst.one_shot = true;
	inst.transform = global_transform;
	Global.spawn_other(inst);
