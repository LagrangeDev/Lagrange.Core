namespace Lagrange.Core.Internal.Event.Message;

internal class RecallFriendMessageEvent : ProtocolEvent
{
    public string TargetUid { get; } = string.Empty;
    
    public uint ClientSeq { get; }
    
    public uint MessageSeq { get; }
    
    public uint Random { get; }
    
    public uint Timestamp { get; }
    
    private RecallFriendMessageEvent(string targetUid, uint clientSeq, uint messageSeq, uint random, uint timestamp) : base(true)
    {
        TargetUid = targetUid;
        MessageSeq = messageSeq;
        ClientSeq = clientSeq;
        Random = random;
        Timestamp = timestamp;
    }

    private RecallFriendMessageEvent(int resultCode) : base(resultCode) { }
    
    public static RecallFriendMessageEvent Create(string targetUid, uint clientSeq, uint messageSeq, uint random, uint timestamp) => 
        new(targetUid, clientSeq, messageSeq, random, timestamp);
    
    public static RecallFriendMessageEvent Result(int resultCode) => new(resultCode);
}