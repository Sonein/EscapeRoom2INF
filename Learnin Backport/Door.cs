using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Learnin.Statics;

namespace Learnin;

public class Door : Polygon2D
{
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private bool _unlocked;
	private Vector2 _ironMouseOffset;
	private List<string> _locks;
	private MovementManager _movementManager;

	public override void _Ready()
	{
		_locks = new List<string>();
		_unlocked = true;
		_movementManager = MovementManager.Instance;
		_movementManager.Add(this);
	}

	public override void _Process(float delta)
	{
		if (_isDragging)
		{
			_isDragging = _movementManager.CanMove(this);
			FollowIronMouse();
		}

		if (_inGame && _unlocked)
		{
			GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
			_movementManager.Remove(this);
			QueueFree();
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		//TODO safety pri davani kodu
		if (@event is InputEventKey eventKey && eventKey.Pressed && _isSelected)
		{
			switch (eventKey.Scancode)
			{
				case 68:
					SigKill();
					break;
			}
		}
	}

	private void OnArea2DInputEvent(object viewport, object @event, int shape_idx)
	{
		if (!_inGame)
		{
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == 1)
			{
				_isDragging = _movementManager.StartMove(@event as InputEventMouseButton, GetGlobalMousePosition(), Position, this);
				if (_isDragging)
				{
					_ironMouseOffset = _movementManager.IronmouseOffset;
				}
			}
			else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == 2)
			{
				if ((@event as InputEventMouseButton).IsPressed())
				{
					SetInternals("select");
				}
			}
			if (@event is InputEventMouseButton bb && bb.Doubleclick)
			{
				GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");	
				GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
			}
		}
	}
	
	private void FollowIronMouse()
	{
		Position = GetGlobalMousePosition() + _ironMouseOffset;
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
	
	private void AddLock(Node l)
	{
		if (_locks.Contains(l.Name)) {return;}
		_locks.Add(l.Name);
		_unlocked = false;
	}

	private void RemoveLock(Node l)
	{
		if (!_locks.Contains(l.Name)) {return;}
		_locks.Remove(l.Name);
		if (!_locks.Any())
		{
			_unlocked = true;
		}
	}
	
	private string GetShapeType()
	{
		return "door";
	}

	private void SigKill()
	{
		GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
		GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");
		GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
		foreach (var lock1 in _locks)
		{
			GetNode<Polygon2D>("/root/Main/" + lock1).Call("RemoveDoor", Name);
		}
		_movementManager.Remove(this);
		QueueFree();
	}
}
