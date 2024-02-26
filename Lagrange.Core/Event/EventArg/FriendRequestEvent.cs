namespace Lagrange.Core.Event.EventArg;

public class FriendRequestEvent : EventBase
{
    public uint SourceUin { get; }
    
    internal string SourceUid { get; }
    
    public string Name { get; }
    
    public string Message { get; }
    
    internal FriendRequestEvent(uint sourceUin, string sourceUid, string name, string message)
    {
        SourceUin = sourceUin;
        SourceUid = sourceUid;
        Name = name;
        Message = message;
        
        EventMessage = $"[{nameof(FriendRequestEvent)}]: {SourceUin}:{Name} {Message}";
    }
}