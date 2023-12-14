using Godot;

namespace Learnin;

public class ObjectConnector
{

    public static void Connect(Node caller, string callerType, Node called)
    {
        switch (callerType)
        {
            case "key":
                caller.Call("AddLock", called.Name);
                called.Call("AddKey", caller);
                break;
            case "lock":
                caller.Call("AddDoor", called.Name);
                called.Call("AddLock", caller);
                break;
            case "code":
                caller.Call("AddDoor", called.Name);
                called.Call("AddLock", caller);
                break;
        }
    }

    public static void Disconnect(Node caller, string callerType, Node called)
    {
        switch (callerType)
        {
            case "key":
                caller.Call("RemoveLock", called.Name);
                called.Call("RemoveKey", caller);
                break;
            case "lock":
                caller.Call("RemoveDoor", called.Name);
                called.Call("RemoveLock", caller);
                break;
            case "code":
                caller.Call("RemoveDoor", called.Name);
                called.Call("RemoveLock", caller);
                break;
        }
    }
}