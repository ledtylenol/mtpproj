using Godot;
using System;

public partial class WorldBorder : StaticBody2D
{
	[Export]
	private float Separation { get; set; }
	[Export]
	private World2d World { get; set; }

	[Export]
	private CollisionShape2D Left { get; set; }
	[Export]
	private CollisionShape2D Right { get; set; }
	[Export]
	private CollisionShape2D Up { get; set; }
	[Export]
	private CollisionShape2D Down { get; set; }


	public override void _Ready()
	{
		base._Ready();
		World.Connect("PlayAreaUpdated", Callable.From<Rect2>(UpdateShapes));
		UpdateShapes(World.PlayArea);
	}


	private void UpdateShapes(Rect2 newArea)
	{
		var dX = -newArea.Size.X / 2f;
		var dY = -newArea.Size.Y / 2f;

		var vert = new Vector2(0f, dY - Separation);
		var hor = new Vector2(dX - Separation, 0f);

		Left.Position = -hor;
		Right.Position = hor;

		Up.Position = -vert;
		Down.Position = vert;
	}
}
