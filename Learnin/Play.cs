using Godot;

namespace Learnin;

public partial class Play : Button
{
	private bool _inGame;
	
	public override void _Ready()
	{
	}
	
	public override void _Process(double delta)
	{
		this.Text = _inGame ? "UNPLAY" : "PLAY";
	}
	
	private void _on_button_down()
	{
		_inGame = !_inGame;
		GetNode<Node>("/root/Main/Menu/ItemSelection/OpenMenu").Call("SetInternals", "play");
		GetNode<Node>("/root/Main/Menu/ItemList/ListMenu").Call("SetInternals", "play");
		GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("SetInternals", "play");
		GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("SetInternals", "play");
		var node = GetNode<Node>("/root/Main/Menu/ItemList/ListMenu");
		var nodes = node.Call("GetNodes").AsGodotArray();
		foreach (Node x in nodes)
		{
			GD.Print(x.Name);
			x.Call("SetInternals", "play");
		}
	}

}
