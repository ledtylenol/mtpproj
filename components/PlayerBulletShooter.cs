using Godot;
using System;

[GlobalClass]
public partial class PlayerBulletShooter : BulletShooter
{
	[Export]
	public Cursor Cursor { get; set; }

	[Export]
	public uint CooldownItems
	{
		get => _CooldownItems; set
		{
			_CooldownItems = Math.Clamp(value, 0, 4);
		}
	}

	private uint _CooldownItems;

	[Export]
	public uint CountItems
	{
		get => _CountItems; set
		{
			_CountItems = Math.Clamp(value, 0, 2);
		}
	}

	private uint _CountItems;

	[Export]
	public uint RangeItems
	{
		get => _RangeItems; set
		{
			_RangeItems = Math.Clamp(value, 0, 4);
		}
	}

	private uint _RangeItems;

	public override float GetBulletCount()
	{
		return BulletCount + CountItems;
	}

	public override float GetCooldown()
	{
		return Cooldown - CooldownItems / 50f;
	}

	public override Vector2 GetDir()
	{
		return Cursor.MouseDir;
	}

	public override float GetSpread()
	{
		return Spread;
	}
	public override float GetRange()
	{
		return Range + RangeItems * 50f;
	}
	public void Reset()
	{
		CountItems = 0;
		RangeItems = 0;
		CooldownItems = 0;
	}

}
