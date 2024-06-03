namespace Lagrange.Core.Event.EventArg;

public class FriendRequestEvent : EventBase
{
    public uint SourceUin { get; }
    
    internal string SourceUid { get; }
    
    public string Source { get; }
    
    public string Message { get; }
    
    internal FriendRequestEvent(uint sourceUin, string sourceUid, string message, string source)
    {
        SourceUin = sourceUin;
        SourceUid = sourceUid;
        Message = message;
        Source = source;
        
        EventMessage = $"[{nameof(FriendRequestEvent)}]: {SourceUin}:{Source} {Message}";
    }
}