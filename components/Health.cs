using Godot;
using System;

[GlobalClass]
public partial class Health : Node
{
	private float _Health;
	[Export]
	public float CurrentHealth
	{
		get => _Health;
		set
		{
			_Health = Mathf.Clamp(value, 0f, MaxHealth);

		}
	}

	private float _MaxHealth;
	[Export]
	public float MaxHealth
	{
		get => _MaxHealth;
		set
		{
			_MaxHealth = value;
			CurrentHealth = Mathf.Min(CurrentHealth, value);
		}
	}

	[Signal]
	public delegate void MaxHealthChangedEventHandler(Entity entity, float oldHealth, float newHealth);

	[Signal]
	public delegate void HealthChangedEventHandler(Entity entity, float oldHealth, float newHealth);

	[Signal]
	public delegate void DamagedEventHandler(Entity entity, HitBox source, float damage);

	[Signal]
	public delegate void HealedEventHandler(Entity entity, HitBox source, float damage);

	public void Hit(HurtBox hurtBox, HitBox source)
	{
	}
}
