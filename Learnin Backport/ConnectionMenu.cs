using Godot;
using System;
using Godot.Collections;
using Learnin.Statics;

namespace Learnin;

public class ConnectionMenu : MenuButton
{
	
	private PopupMenu _popupMenu;
	private bool _inGame;
	private int _id;
	private Node _toConnectTo;
	private string _toConnectToType;
	
	public override void _Ready()
	{
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
			ObjectConnector.Connect(_toConnectTo, _toConnectToType, node);
			GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("AddItem", name);
		}
	}

	private void GenerateOptions(Node node, string type)
	{
		ClearSelf();
		_toConnectTo = node;
		if (node.Call("GetShapeType") is string nn)
		{
			_toConnectToType = nn;
		}
		var nodes = Caster.CastToArrayPoly2D(GetNode<Node>("/root/Main/Menu/ItemList/ListMenu").Call("GetNodes"));
		//GD.Print(nodes);
		foreach (Polygon2D x in nodes)
		{
			string nodeType = (string)x.Call("GetShapeType");
			//GD.Print(nodeType + " " + _toConnectToType);
			if (nodeType.Equals(type))
			{
				_popupMenu.AddItem(x.Name, _id++);
			}
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
		_popupMenu.Clear();
		_id = 0;
	}
}
