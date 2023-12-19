namespace Lagrange.Core.Event.EventArg;

public class FriendRequestEvent : EventBase
{
    public uint SourceUin { get; }
    
    public string Name { get; }
    
    public string Message { get; }
    
    internal FriendRequestEvent(uint sourceUin, string name, string message)
    {
        SourceUin = sourceUin;
        Name = name;
        Message = message;
        
        EventMessage = $"[{nameof(FriendRequestEvent)}]: {SourceUin}:{Name} {Message}";
    }
}