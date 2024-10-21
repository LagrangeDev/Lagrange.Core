namespace Lagrange.Core.Internal.Event.Action;

internal class JoinEmojiChainEvent : ProtocolEvent
{
    public uint TargetMessageSeq { get; set; }
    
    public uint TargetFaceId { get; set; }
    
    public uint? GroupUin { get; set; }
    
    public string? FriendUid { get; set; }

    protected JoinEmojiChainEvent(uint targetMessageSeq, uint targetFaceId) : base(true)
    {
        TargetMessageSeq = targetMessageSeq;
        TargetFaceId = targetFaceId;
    }
    
    protected JoinEmojiChainEvent(int resultCode) : base(resultCode) { }
    
    public static JoinEmojiChainEvent Result(int resultCode) => new(resultCode);
}
