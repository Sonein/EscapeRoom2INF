using Godot;
using System;

namespace Learnin;

public class Main : Node2D
{
	
	public override void _Ready()
	{
		foreach (var x in this.GetChildren())
		{
			if (x is CanvasItem x1)
			{
				x1.Visible = false;
			}
		}
		PackedScene packedScene;
		Polygon2D temp = null;
		packedScene = (PackedScene)GD.Load("res://login.tscn");
		temp = (Polygon2D)packedScene.Instance();
		temp.Name = "Login";
		temp.Position = new Vector2(760, 440);
		GetNode<Node>("/root/Main").AddChild(temp);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
