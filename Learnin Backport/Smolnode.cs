using Godot;
using System;

namespace Learnin;

public class Smolnode : Polygon2D
{
	private int _state;
	private int _id;
	private bool _inGame;

	public override void _Ready()
	{
		if (this.Color != Colors.PaleVioletRed)
		{
			this._state = 0;
			this.Color = Colors.SkyBlue;
		}
	}

	public override void _Process(float delta)
	{
	}
	
	private void OnArea2DInputEvent(object viewport, object @event, int shape_idx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == 1 && mouseEvent.IsPressed())
		{
			if (_inGame)
			{
				if (this.Color == Colors.SkyBlue)
				{
					//TODO iba ak je v chaine
					if ((bool)GetParent().GetParent().Call("CanAdd", _id))
					{
						this.Color = Colors.PaleGreen;
						this._state = 2;
					}
				}
				else
				{
					if ((bool)GetParent().GetParent().Call("CanRemove", _id))
					{
						this.Color = Colors.SkyBlue;
						this._state = 0;
					}
				}
			}
			else
			{
				if (this.Color == Colors.SkyBlue)
				{
					this.Color = Colors.PaleVioletRed;
					this._state = 1;
				}
				else
				{
					this.Color = Colors.SkyBlue;
					this._state = 0;
				}
			}
		}
	}
	
	private void SetSpecial(string special)
	{
		this._id = int.Parse(special);
	}

	private int GetState()
	{
		return this._state;
	}
	
	private void SetState(int ina)
	{
		this._state = ina;
		if (!_inGame && ina == 1)
		{
			this.Color = Colors.PaleVioletRed;
		}
	}

	private void Gamin()
	{
		_inGame = !_inGame;
	}
}
