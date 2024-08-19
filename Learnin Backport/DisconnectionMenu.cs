using Godot;
using System;
using Learnin.Statics;

namespace Learnin;

public class DisconnectionMenu : MenuButton
{
	
	private System.Collections.Generic.Dictionary<string, int> _items;
	private PopupMenu _popupMenu;
	private bool _inGame;
	private int _id;
	private Node _toConnectTo;
	private string _toConnectToType;
	
	public override void _Ready()
	{
		_items = new System.Collections.Generic.Dictionary<string, int>();
		_popupMenu = this.GetPopup();
		_popupMenu.Connect("id_pressed", this, "OnMenuItemSelected");
		_id = 0;
	}

	public override void _Process(float delta)
	{
	}
	
	private void OnMenuItemSelected(int id)
	{
		if (!_inGame)
		{
			int index = _popupMenu.GetItemIndex(id);
			string name = _popupMenu.GetItemText(index);
			Node node = GetNode<Node>("/root/Main/" + name);
			ObjectConnector.Disconnect(_toConnectTo, _toConnectToType, node);
			RemoveItem(name);
		}
	}

	private void AddItem(string x)
	{
		if (_items.ContainsKey(x))
		{
			return;
		}
		_items.Add(x, _id);
		_popupMenu.AddItem(x, _id++);
	}

	private void RemoveItem(string x)
	{
		int id;
		_items.TryGetValue(x, out id);
		int index = _popupMenu.GetItemIndex(id);
		_items.Remove(x);
		_popupMenu.RemoveItem(index);
	}

	private void ResetSelf(Node x, string type)
	{
		var has = Caster.CastToArrayString(x.Call("GiveUpYourList"));
		ClearSelf();
		_toConnectTo = x;
		_toConnectToType = type;
		foreach (string kirsch in has)
		{
			//GD.Print(kirsch);
			AddItem(kirsch);
		}
	}

	private void SetInternals(string x)
	{
		switch (x)
		{
			case "play":
				_inGame = !_inGame;
				ClearSelf();
				break;
		}
	}
	
	private void ClearSelf()
	{
		_toConnectTo = null;
		_items.Clear();
		_popupMenu.Clear();
		_id = 0;
	}
}
