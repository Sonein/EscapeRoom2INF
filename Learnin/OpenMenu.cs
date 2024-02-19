using Godot;
using Learnin.Statics;

namespace Learnin;

public partial class OpenMenu : MenuButton
{
	private int _id;

	private bool _inGame;
	
	public override void _Ready()
	{
		Callable callable = new Callable(this, nameof(OnMenuItemSelected));
		var popupMenu = this.GetPopup();
		popupMenu.Connect("id_pressed", callable);
	}
	
	public override void _Process(double delta)
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
