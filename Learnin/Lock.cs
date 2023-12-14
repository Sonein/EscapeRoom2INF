using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Learnin;

public partial class Lock : Polygon2D
{
	
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private bool _unlocked;
	private string _type;
	private Vector2 _ironMouseOffset;
	private System.Collections.Generic.Dictionary<string, bool> _accepts;
	private List<string> _keys;
	private List<string> _doors;
	
	public override void _Ready()
	{
		_accepts = new System.Collections.Generic.Dictionary<string, bool>();
		_keys = new List<string>();
		_doors = new List<string>();
		_accepts.Add("key", false);
		_accepts.Add("lock", false);
		_accepts.Add("door", true);
		_unlocked = true;
		_type = "lock";
	}
	
	public override void _Process(double delta)
	{
		if (_isDragging)
		{
			FollowIronMouse();
		}

		if (_inGame && _unlocked)
		{
			foreach (var door in _doors)
			{
				GetNode<Polygon2D>("/root/Main/" + door).Call("RemoveLock", this);
			}
			GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
			QueueFree();
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed && _isSelected)
		{
			switch (eventKey.Keycode)
			{
				case Godot.Key.D:
					GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
					GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");
					GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
					if (_keys.Any())
					{
						foreach (var key in _keys)
						{
							GetNode<Polygon2D>("/root/Main/" + key).Call("RemoveLock", Name);
						}
					}
					if (_doors.Any())
					{
						foreach (var door in _doors)
						{
							GetNode<Polygon2D>("/root/Main/" + door).Call("RemoveLock", this);
						}
					}
					QueueFree();
					break;
			}
		}
	}
	
	private void _on_area_2d_input_event(Node viewport, InputEvent @event, long shape_idx)
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
				GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("GenerateOptions", this, "door");
				GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ResetSelf", this, "lock");
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

	private void AddKey(Node key)
	{
		if (_keys.Contains(key.Name)) {return; }
		_keys.Add(key.Name);
		_unlocked = false;
	}

	private void RemoveKey(Node key)
	{
		if (!_keys.Contains(key.Name)) {return; }
		_keys.Remove(key.Name);
		if (!_keys.Any())
		{
			_unlocked = true;
		}
	}

	private void AddDoor(string door)
	{
		if (_doors.Contains(door)) return;
		_doors.Add(door);
		//GetNode<Polygon2D>("/root/Main/" + door).Call("AddLock", this);
	}

	private void RemoveDoor(string door)
	{
		if (!_doors.Contains(door)) return;
		_doors.Remove(door);
		//GetNode<Polygon2D>("/root/Main/" + door).Call("RemoveLock", this);
	}

	private Array<string> GiveUpYourList()
	{
		return new Array<string>(_doors);
	}
	
	private string GetShapeType()
	{
		return _type;
	}
}
