using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Learnin.Ciphers;
using Learnin.Statics;

namespace Learnin;

public class CipherLock : Polygon2D
{
	private bool _isSelected;
	private bool _isDragging;
	private bool _inGame;
	private bool _unlocked;
	private string _code;
	private string _eCode;
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
		_movementManager.Add(this);
		((RichTextLabel)((Node2D)this.GetChildren()[3]).GetChildren()[0]).Text = CipherCatalogue.GetTooltip("def");
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
		if (_inGame && _code.Equals(_dynamicText.Text))
		{
			GD.Print(_code + " " + _dynamicText.Text);
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
				GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("ResetSelf", this, "cipher");
			}
		}
	}


	private void OnTextEdit2TextChanged()
	{
		TextEdit from = (TextEdit)((Node2D)this.GetChildren()[2]).GetChildren()[1];
		RichTextLabel to = (RichTextLabel)((Node2D)GetChildren()[3]).GetChildren()[0];
		to.Text = CipherCatalogue.GetTooltip(from.Text);
		//GD.Print(from.Text);
	}


	private void OnButtonButtonDown()
	{
		string pass =  ((TextEdit)((Node2D)this.GetChildren()[2]).GetChildren()[0]).Text;
		string cipher = ((TextEdit)((Node2D)this.GetChildren()[2]).GetChildren()[1]).Text;
		string key = ((TextEdit)((Node2D)this.GetChildren()[2]).GetChildren()[2]).Text;
		_cipher = CipherCatalogue.GetCipher(cipher);
		_key = key;
		_eCode = _cipher.Encrypt(pass, key);
		_code = pass;
		((Label)GetChildren()[4]).Text = _eCode;
		GD.Print(_code);
	}
	
	private void FollowIronMouse()
	{
		Position = this.GetGlobalMousePosition() + _ironMouseOffset;
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
					_dynamicText.Text = "";
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
		if (_cipher == null)
		{
			_cipher = new Default();
		}
		GD.Print(_cipher.Type());
		if (_key == null || _key.Equals(""))
		{
			_key = "$NULL$";
		}
		if (_code == null || _code.Equals(""))
		{
			_code = "$NULL$";
		}
		return _code + " " + _key + " " + _cipher.Type(); 
	}

	private void SetSpecial(string special)
	{
		Scanner scanner = new Scanner(special);
		string code = scanner.Next();
		string key = scanner.Next();
		string cipher = scanner.Next();
		_cipher = CipherCatalogue.GetCipher(cipher);
		_key = key;
		if (code != "$NULL$")
		{
			_code = code;
			_eCode = _cipher.Encrypt(code, _key);
			((Label)GetChildren()[4]).Text = _eCode;
		}
		
	}
}
