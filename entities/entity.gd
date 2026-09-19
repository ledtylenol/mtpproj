class_name Entity extends CharacterBody2D
@export var move_speed: float 

@export var acceleration: float

@export var friction: float 

@export var counts_toward_enemies := true;

@export var state_machine: StateMachine 

@export var health: Health 

var normal: Vector2  
func move(_delta: float) -> void:
	move_and_slide();

func _physics_process(delta: float) -> void:
	if state_machine: state_machine.physics_tick(delta);
func _process(delta: float) -> void:
	if state_machine: state_machine.tick(delta);
