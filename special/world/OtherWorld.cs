using Godot;
using System;

[GlobalClass]
public partial class OtherWorld : SubViewportContainer
{
	[Export]
	public Node2D Root { get; set; }

	[Export]
	public SubViewport Viewport { get; set; }
	public override void _Ready()
	{
		base._Ready();
		Global.Single.Connect("SpawnOtherWorld", Callable.From<Node2D>(SpawnNode));
		LevelHandler.Single.Connect("BossFinished", Callable.From(ClearViewport));
		Global.World.Connect("PlayAreaUpdated", Callable.From<Rect2>(UpdateSize));
		UpdateSize(Global.World.PlayArea);
		Global.World.PlayerDied += (player) => ClearViewport();
	}

	private void SpawnNode(Node2D node)
	{
		Root.AddChild(node);
	}
	private void ClearViewport()
	{
		foreach (var child in Root.GetChildren()) if (child is Node2D node) node.Hide();
		Viewport.RenderTargetClearMode = SubViewport.ClearMode.Once;

	}

	private void UpdateSize(Rect2 newSize)
	{
		Position = newSize.Position - new Vector2(120f, 0f);
		Size = newSize.Size + new Vector2(240f, 0f);
	}
}
