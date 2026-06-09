using Godot;
using System;

[GlobalClass]
public partial class Health : Node
{
	private int _Health;
	[Export]
	public int CurrentHealth
	{
		get => _Health;
		set
		{
			var oldHealth = _Health;
			_Health = Mathf.Clamp(value, 0, MaxHealth);
			EmitSignalHealthChanged((Entity)Owner, oldHealth, _Health);

		}
	}

	private int _MaxHealth;
	[Export]
	public int MaxHealth
	{
		get => _MaxHealth;
		set
		{
			_MaxHealth = value;
			CurrentHealth = Mathf.Min(CurrentHealth, value);
		}
	}
	[Export]
	public float IFrames { get; set; }

	[Signal]
	public delegate void MaxHealthChangedEventHandler(Entity entity, float oldHealth, float newHealth);

	[Signal]
	public delegate void HealthChangedEventHandler(Entity entity, float oldHealth, float newHealth);

	[Signal]
	public delegate void DamagedEventHandler(Entity entity, HitBox source, float damage);

	[Signal]
	public delegate void HealedEventHandler(Entity entity, HitBox source, float damage);

	[Signal]
	public delegate void DiedEventHandler(Entity entity);

	[Signal]
	public delegate void AlreadyDeadEventHandler(Entity entity);

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		base._Ready();
	}
	public void Hit(HurtBox hurtBox, HitBox source)
	{
		if (CurrentHealth == 0) { EmitSignalAlreadyDead((Entity)Owner); return; }
		CurrentHealth -= source.Damage;
		EmitSignalDamaged((Entity)Owner, source, source.Damage);
		if (CurrentHealth == 0) EmitSignalDied((Entity)Owner);
	}

}
