namespace Lagrange.Core.Internal.Event.Message;

internal class RecallFriendMessageEvent : ProtocolEvent
{
    public string TargetUid { get; } = string.Empty;
    
    public ulong ClientSeq { get; }
    
    public ulong MessageSeq { get; }
    
    public uint Random { get; }
    
    public uint Timestamp { get; }
    
    private RecallFriendMessageEvent(string targetUid, ulong clientSeq, ulong messageSeq, uint random, uint timestamp) : base(true)
    {
        TargetUid = targetUid;
        MessageSeq = messageSeq;
        ClientSeq = clientSeq;
        Random = random;
        Timestamp = timestamp;
    }

    private RecallFriendMessageEvent(int resultCode) : base(resultCode) { }
    
    public static RecallFriendMessageEvent Create(string targetUid, ulong clientSeq, ulong messageSeq, uint random, uint timestamp) => 
        new(targetUid, clientSeq, messageSeq, random, timestamp);
    
    public static RecallFriendMessageEvent Result(int resultCode) => new(resultCode);
}