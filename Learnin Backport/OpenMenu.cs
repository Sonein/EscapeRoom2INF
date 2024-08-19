using Godot;
using System;
using Learnin.Statics;

namespace Learnin;

public class OpenMenu : MenuButton
{
	
	/* Needs font_size = 45 and 7 items. 
	item_count = 7
	popup/item_0/text = "Square"
	popup/item_0/id = 0
	popup/item_1/text = "Door"
	popup/item_1/id = 1
	popup/item_2/text = "Code Lock"
	popup/item_2/id = 2
	popup/item_3/text = "Lock"
	popup/item_3/id = 3
	popup/item_4/text = "Key"
	popup/item_4/id = 4
	popup/item_5/text = "Cipher Lock"
	popup/item_5/id = 5
	popup/item_6/text = "Graph Lock"
	popup/item_6/id = 6
	*/
	
	private int _id;

	private bool _inGame;
	
	public override void _Ready()
	{
		
		var popupMenu = this.GetPopup();
		popupMenu.AddItem("Square", 0);
		popupMenu.AddItem("Door", 1);
		popupMenu.AddItem("Code Lock", 2);
		popupMenu.AddItem("Lock", 3);
		popupMenu.AddItem("Key", 4);
		popupMenu.AddItem("Cipher Lock", 5);
		popupMenu.AddItem("Graph Lock", 6);
		//GD.Print(this.HasFont("font", ""));
		if (this.GetFont("font", "") is DynamicFont font)
		{
			font.Size = 45;
			this.AddFontOverride("font", font);
		}
		popupMenu.Connect("id_pressed", this, "OnMenuItemSelected");
	}

	public override void _Process(float delta)
	{
	}
	
	private void OnMenuItemSelected(int id)
	{
		if (!_inGame)
		{
			var position = GetGlobalMousePosition() - new Vector2(100, 100);
			Polygon2D tempPolygon = null;
			string name;
			switch (id)
			{
				case 0:
					name = "Square" + _id;
					tempPolygon = ObjectCreator.Create(name, "none", position, null);
					break;
				case 1:
					name = "Door" + _id;
					tempPolygon = ObjectCreator.Create(name, "door", position, null);
					break;
				case 2:
					name = "CodeLock" + _id;
					tempPolygon = ObjectCreator.Create(name, "code", position, null);
					break;
				case 3:
					name = "Lock" + _id;
					tempPolygon = ObjectCreator.Create(name, "lock", position, null);
					break;
				case 4:
					name = "Key" + _id;
					tempPolygon = ObjectCreator.Create(name, "key", position, null);
					break;
				case 5:
					name = "CipherLock" + _id;
					tempPolygon = ObjectCreator.Create(name, "cipher", position, null);
					break;
				case 6:
					name = "GraphLock" + _id;
					tempPolygon = ObjectCreator.Create(name, "graph", position, null);
					break;
			}

			if (tempPolygon != null)
			{
				_id++;
				GetNode<Node>("/root/Main").AddChild(tempPolygon);
				GetNode<Node>("/root/Main/Menu/ItemList/ListMenu").Call("AddItem", tempPolygon);
			}
		}
	}
	
	public void SetInternals(string x)
	{
		switch (x)
		{
			case "play":
				_inGame = !_inGame;
				break;
		}
	}
}
