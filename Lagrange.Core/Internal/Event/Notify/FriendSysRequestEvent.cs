namespace Lagrange.Core.Internal.Event.Notify;

internal class FriendSysRequestEvent : ProtocolEvent
{
    public uint SourceUin { get; }
    
    public string SourceUid { get; }
    
    public string Message { get; }
    
    public string Source { get; }

    private FriendSysRequestEvent(uint sourceUin, string sourceUid, string message, string source) : base(0)
    {
        SourceUin = sourceUin;
        SourceUid = sourceUid;
        Message = message;
        Source = source;
    }

    public static FriendSysRequestEvent Result(uint sourceUin, string sourceUid, string message, string source) =>
        new(sourceUin, sourceUid, message, source);
}