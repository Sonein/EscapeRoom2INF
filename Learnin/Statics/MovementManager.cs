using System.Collections.Generic;
using Godot;

namespace Learnin.Statics;

public sealed class MovementManager
{
    public Vector2 IronmouseOffset;
    private Dictionary<Node, bool> _actives;
    private Node _top;

    private MovementManager()
    {
        _actives = new Dictionary<Node, bool>();
        _top = null;
    }

    public static MovementManager Instance
    {
        get { return MmContainer.Instance; }
    }
    
    private class MmContainer
    {
        static MmContainer()
        {
        }

        internal static readonly MovementManager Instance = new MovementManager();
    }

    public bool StartMove (InputEvent @event, Vector2 globalPos, Vector2 localPos, Node self)
    {
        
        if (@event.IsPressed())
        {
            IronmouseOffset = localPos - globalPos;
            _actives[self] = true;
            return true;
        }
        else if (@event.IsReleased())
        {
            _actives[self] = false;
            if (self == _top)
            {
                _top = null;
            }
            return false;
        }
        else
        {
            _actives[self] = false;
            if (self == _top)
            {
                _top = null;
            }
            return false;
        }
    }

    public bool CanMove(Node node)
    {
        if (node == _top)
        {
            return true;
        }
        foreach (var pair in _actives)
        {
            if (pair.Key == node)
            {
                _top = node;
                return true;
            }
            
            if (pair.Value)
            {
                break;
            }
        }
        _actives[node] = false;
        return false;
    }

    public void Add(Node node)
    {
        _actives.Add(node, false);
    }

    public void Remove(Node node)
    {
        _actives.Remove(node);
    }
    
}