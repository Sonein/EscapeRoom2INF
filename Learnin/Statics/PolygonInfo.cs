using Godot;
using Godot.Collections;

namespace Learnin.Statics;

public struct PolygonInfo
{
    public PolygonInfo(string name, string type, Vector2 position, Array<string> array, string special)
    {
        Name = name;
        Type = type;
        Position = position;
        Connections = array.Duplicate();
        Special = special;
    }

    public string Name { get; }
    public string Type { get; }
    public Vector2 Position { get; }
    public Array<string> Connections { get; }
    public string Special { get; }
}