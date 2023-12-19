namespace Lagrange.Core.Internal.Event.Notify;

internal class FriendSysRequestEvent : ProtocolEvent
{
    public uint SourceUin { get; }
    
    public string SourceUid { get; }
    
    public string Message { get; }
    
    public string Name { get; }

    private FriendSysRequestEvent(uint sourceUin, string sourceUid, string message, string name) : base(0)
    {
        SourceUin = sourceUin;
        SourceUid = sourceUid;
        Message = message;
        Name = name;
    }

    public static FriendSysRequestEvent Result(uint sourceUin, string sourceUid, string message, string name) =>
        new(sourceUin, sourceUid, message, name);
}