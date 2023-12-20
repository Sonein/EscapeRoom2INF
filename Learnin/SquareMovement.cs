using System;
using System.Collections.Generic;
using Godot;

namespace Learnin;

public partial class SquareMovement : Polygon2D
{
	private Color _color;
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private Vector2 _ironMouseOffset;
	private Dictionary<String, bool> _accepts;
	private string _type;
	
	public override void _Ready()
	{
		this._color = this.Color;
		this._accepts = new Dictionary<string, bool>();
		_accepts.Add("key", false);
		_accepts.Add("lock", false);
		_accepts.Add("door", false);
		_type = "none";
	}
	
	public override void _Process(double delta)
	{
		if (_isDragging)
		{
			FollowIronMouse();
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed && _isSelected)
		{
			switch (eventKey.Keycode)
			{
				case Godot.Key.D:
					SigKill();
					break;
			}
		}
	}

	private void FollowIronMouse()
	{
		this.Position = GetGlobalMousePosition() + _ironMouseOffset;
	}


	private void SetInternals(string x)
	{
		switch (x)
		{
			case "select":
				_isSelected = !_isSelected;
				break;
			case "drag":
				_isDragging = !_isDragging;
				break;
			case "play":
				_inGame = !_inGame;
				break;
		}
	}
	
	private void _on_area_2d_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (!_inGame)
		{
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				if (@event.IsPressed())
				{
					_ironMouseOffset = this.Position - GetGlobalMousePosition();
					_isDragging = true;
				}
				else if (@event.IsReleased())
				{
					_isDragging = false;
				}
				else
				{
					_isDragging = false;
				}
			}
			else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Right)
			{
				if (@event.IsPressed())
				{
					SetInternals("select");
				}
			}
			if (@event is InputEventMouseButton { DoubleClick: true } && !_inGame)
			{
				GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");	
				GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
			}
		}
	}

	private string GetShapeType()
	{
		return _type;
	}

	private void SigKill()
	{
		GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
		QueueFree();
	}
}
