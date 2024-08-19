using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Learnin.Statics;

namespace Learnin;

public class CodeLock : Polygon2D
{
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private bool _unlocked;
	private string _code;
	private Vector2 _ironMouseOffset;
	private List<string> _doors;
	private TextEdit _dynamicText;
	private MovementManager _movementManager;

	public override void _Ready()
	{
		_doors = new List<string>();
		_dynamicText = (TextEdit)GetChildren()[0];
		_movementManager = MovementManager.Instance;
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
			foreach (var door in _doors)
			{
				GetNode<Polygon2D>("/root/Main/" + door).Call("RemoveLock", this);
			}
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
	
	private void OnTextEditTextChanged()
	{
		if (!_inGame)
		{
			_code = _dynamicText.Text;
		}
		if (_inGame && _code.Equals(_dynamicText.Text))
		{
			//GD.Print(_code + " " + _dynamicText.Text);
			_unlocked = true;
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
				GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("GenerateOptions", this, "door");
				GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ResetSelf", this, "code");
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
				if (_inGame)
				{
					_dynamicText.Text = "";
					if (string.IsNullOrEmpty(_code))
					{
						_unlocked = true;
					}
				}
				break;
		}
	}
	
	private void AddDoor(string door)
	{
		if (_doors.Contains(door)) return;
		_doors.Add(door);
	}

	private void RemoveDoor(string door)
	{
		if (!_doors.Contains(door)) return;
		_doors.Remove(door);
	}
	
	private Array<string> GiveUpYourList()
	{
		return new Array<string>(_doors);
	}
	
	private string GetShapeType()
	{
		return "code";
	}

	private string GetSpecial()
	{
		return _code;
	}

	private void SetSpecial(string code)
	{
		_code = new Scanner(code).Next();
	}

	private void SigKill()
	{
		GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
		GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");
		GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
		foreach (var door in _doors)
		{
			GetNode<Polygon2D>("/root/Main/" + door).Call("RemoveLock", this);
		}
		_movementManager.Remove(this);
		QueueFree();
	}
}
