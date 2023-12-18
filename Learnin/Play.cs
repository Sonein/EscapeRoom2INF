using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Learnin;

public partial class Play : Button
{
	private bool _inGame;
	private GameSaver _gameSaver;
	
	public override void _Ready()
	{
		_gameSaver = new GameSaver();
	}
	
	public override void _Process(double delta)
	{
		this.Text = _inGame ? "UNPLAY" : "PLAY";
	}
	
	private void _on_button_down()
	{
		_inGame = !_inGame;
		var node = GetNode<Node>("/root/Main/Menu/ItemList/ListMenu");
		var nodes = node.Call("GetNodes").AsGodotArray<Polygon2D>();
		GetNode<Node>("/root/Main/Menu/ItemSelection/OpenMenu").Call("SetInternals", "play");
		GetNode<Node>("/root/Main/Menu/EditMenu/ConnectionList/ConnectionMenu").Call("SetInternals", "play");
		GetNode<Node>("/root/Main/Menu/EditMenu/DisconnectionList/DisconnectionMenu").Call("SetInternals", "play");
		if (_inGame)
		{
			List<Polygon2D> nodesFr = new List<Polygon2D>();
			foreach (Polygon2D x in nodes)
			{
				x.Call("SetInternals", "play");
				nodesFr.Add(x);
			}
			node.Call("SetInternals", "play");
			_gameSaver.SaveState(nodesFr);
		}
		else
		{
			foreach (var variableNode in nodes)
			{
				//in case, ze mame dalsie problemy s mazanim tak zamenik queuefree
				variableNode.Free();
			}
			nodes.Clear();
			CallDeferred("Rebuild", node);
		} 
	}

	private void Rebuild(Node node)
	{
		node.Call("SetInternals", "play");
		List<PolygonInfo> nodesFr2 = _gameSaver.LoadState();
		foreach (var polygonInfo in nodesFr2)
		{
			Polygon2D tempPolygon = ObjectCreator.Create(polygonInfo.Name, polygonInfo.Type, polygonInfo.Position);
			GetNode<Node>("/root/Main").AddChild(tempPolygon);
			GetNode<Node>("/root/Main/Menu/ItemList/ListMenu").Call("AddItem", tempPolygon);
		}

		foreach (var polygonInfo in nodesFr2)
		{
			Node caller = GetNode<Node>("/root/Main/" + polygonInfo.Name);
			string callerType = polygonInfo.Type;
			foreach (var calledName in polygonInfo.Connections)
			{
				Node called = GetNode<Node>("/root/Main/" + calledName);
				ObjectConnector.Connect(caller, callerType, called);
			}
		}
	}

}
