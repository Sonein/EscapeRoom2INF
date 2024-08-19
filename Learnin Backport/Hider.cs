using Godot;
using System;

namespace Learnin;

public class Hider : Button
{
	
	private bool _left;

	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
	}

	private void OnHiderButtonDown()
	{
		if (!_left)
		{
			this.RectRotation += 180;
			this.RectPosition += new Vector2(48, 48);
			_left = true;
			GetNode<Polygon2D>("/root/Main/Menu").Position += new Vector2(420, 0);
		}
		else
		{
			this.RectRotation -= 180;
			this.RectPosition -= new Vector2(48, 48);
			_left = false;
			GetNode<Polygon2D>("/root/Main/Menu").Position -= new Vector2(420, 0);
		}
	}
}
