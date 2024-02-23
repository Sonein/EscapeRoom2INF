using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Learnin.Ciphers;
using Learnin.Statics;

namespace Learnin;

public partial class Cipherlock : Polygon2D
{
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private bool _unlocked;
	private string _code;
	private string _key;
	private Vector2 _ironMouseOffset;
	private List<string> _doors;
	private TextEdit _dynamicText;
	private MovementManager _movementManager;
	private ICipher _cipher;
	
	public override void _Ready()
	{
		((Polygon2D)GetChildren()[2]).Visible = false;
		((Polygon2D)GetChildren()[3]).Visible = false;
		_doors = new List<string>();
		_dynamicText = (TextEdit)GetChildren()[0];
		_movementManager = MovementManager.Instance;
		((RichTextLabel)GetChildren()[3].GetChildren()[0]).Text = CipherCatalogue.GetTooltip("def");
	}
	
	public override void _Process(double delta)
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
		//@TODO safety pri davani kodu
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
				_isDragging = _movementManager.StartMove(@event, GetGlobalMousePosition(), Position, this);
				if (_isDragging)
				{
					_ironMouseOffset = _movementManager.IronmouseOffset;
				}
			}
			else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Right)
			{
				if (@event.IsPressed())
				{
					SetInternals("select");
				}
			}
			if (@event is InputEventMouseButton { DoubleClick: true })
			{
				GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("GenerateOptions", this, "door");
				GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ResetSelf", this, "code");
			}
		}
	}


	private void _on_text_edit_text_changed()
	{
		if (_inGame && _code.Equals(_dynamicText.Text))
		{
			GD.Print(_code + " " + _dynamicText.Text);
			_unlocked = true;
		}
	}


	private void _on_button_button_down()
	{
		string pass = ((TextEdit)GetChildren()[2].GetChildren()[0]).Text;
		string cipher = ((TextEdit)GetChildren()[2].GetChildren()[1]).Text;
		string key = ((TextEdit)GetChildren()[2].GetChildren()[2]).Text;
		_cipher = CipherCatalogue.GetCipher(cipher);
		_key = key;
		_code = _cipher.Encrypt(pass, key);
		GD.Print(_code);
	}
	
	private void _on_text_edit_2_text_changed()
	{
		TextEdit from = (TextEdit)GetChildren()[2].GetChildren()[1];
		RichTextLabel to = (RichTextLabel)GetChildren()[3].GetChildren()[0];
		to.Text = CipherCatalogue.GetTooltip(from.Text);
		//GD.Print(from.Text);
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
				((Polygon2D)GetChildren()[2]).Visible = _isSelected;
				((Polygon2D)GetChildren()[3]).Visible = _isSelected;
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
					((Polygon2D)GetChildren()[2]).Visible = false;
					((Polygon2D)GetChildren()[3]).Visible = false;
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
		return "cipher";
	}

	private string GetSpecial()
	{
		GD.Print(_cipher.Type());
		return _cipher.Decrypt(_code) + " " + _key + " " + _cipher.Type(); 
	}

	private void SetSpecial(string special)
	{
		Scanner scanner = new Scanner(special);
		string code = scanner.Next();
		string key = scanner.Next();
		string cipher = scanner.Next();
		_cipher = CipherCatalogue.GetCipher(cipher);
		_key = key;
		_code = _cipher.Encrypt(code, _key);
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
