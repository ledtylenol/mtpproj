using Godot;
using System;

[GlobalClass]
public partial class PlayerBulletShooter : BulletShooter
{
	[Export]
	public Cursor Cursor { get; set; }
	public override Vector2 GetDir()
	{
		return Cursor.MouseDir;
	}

}
