using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Learnin;

public partial class CodeLock : Polygon2D
{
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private bool _unlocked;
	private string _code;
	private string _type;
	private Vector2 _ironMouseOffset;
	private System.Collections.Generic.Dictionary<string, bool> _accepts;
	private List<string> _doors;
	private TextEdit _dynamicText;
	
	public override void _Ready()
	{
		_accepts = new System.Collections.Generic.Dictionary<string, bool>();
		_accepts.Add("key", false);
		_accepts.Add("lock", false);
		_accepts.Add("door", true);
		_doors = new List<string>();
		_dynamicText = (TextEdit)this.GetChildren()[0];
		_type = "code";
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
					SigKill();
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
				GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ResetSelf", this, "code");
			}
		}
	}
	
	private void _on_text_edit_text_changed()
	{
		if (!_inGame)
		{
			_code = _dynamicText.Text;
		}
		if (_inGame && _code.Equals(_dynamicText.Text))
		{
			GD.Print(_code + " " + _dynamicText.Text);
			_unlocked = true;
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
				if (_inGame)
				{
					_dynamicText.Clear();
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

	private string GetCode()
	{
		return _code;
	}

	private void SetCode(string code)
	{
		_code = code;
	}

	private void SigKill()
	{
		GetNode<MenuButton>("/root/Main/Menu/ItemList/ListMenu").Call("RemoveItem", this);
		GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("ClearSelf");
		GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ClearSelf");
		if (_doors.Any())
		{
			foreach (var door in _doors)
			{
				GetNode<Polygon2D>("/root/Main/" + door).Call("RemoveLock", this);
			}
		}
		QueueFree();
	}
}
