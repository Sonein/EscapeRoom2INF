using Godot;

namespace Learnin.Statics;

public class ObjectCreator
{
    public static Polygon2D Create(string name, string type, Vector2 position, string special)
    {
        PackedScene packedScene;
        Polygon2D temp = null;
        switch (type)
        {
            case "none":
                packedScene = (PackedScene)GD.Load("res://square.tscn");
                temp = (Polygon2D)packedScene.Instantiate();
                temp.Name = name;
                temp.Position = position;
                break;
            case "door":
                packedScene = (PackedScene)GD.Load("res://door.tscn");
                temp = (Polygon2D)packedScene.Instantiate();
                temp.Name = name;
                temp.Position = position;
                break;
            case "key":
                packedScene = (PackedScene)GD.Load("res://key.tscn");
                temp = (Polygon2D)packedScene.Instantiate();
                temp.Name = name;
                temp.Position = position;
                break;
            case "code":
                packedScene = (PackedScene)GD.Load("res://codelock.tscn");
                temp = (Polygon2D)packedScene.Instantiate();
                temp.Name = name;
                temp.Position = position;
                if (special != null)
                {
                    temp.Call("SetSpecial", special);
                }
                break;
            case "lock":
                packedScene = (PackedScene)GD.Load("res://lock.tscn");
                temp = (Polygon2D)packedScene.Instantiate();
                temp.Name = name;
                temp.Position = position;
                break;
            case "cipher":
                packedScene = (PackedScene)GD.Load("res://cipherlock.tscn");
                temp = (Polygon2D)packedScene.Instantiate();
                temp.Name = name;
                temp.Position = position;
                if (special != null)
                {
                    temp.Call("SetSpecial", special);
                }
                break;
        }
        return temp;
    }
}