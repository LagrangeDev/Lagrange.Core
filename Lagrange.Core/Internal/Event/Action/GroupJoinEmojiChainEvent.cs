namespace Lagrange.Core.Internal.Event.Action;

internal class GroupJoinEmojiChainEvent : JoinEmojiChainEvent
{
    private GroupJoinEmojiChainEvent(uint targetMessageSeq, uint targetFaceId, uint groupUin) : base(targetMessageSeq, targetFaceId)
    {
        GroupUin = groupUin;
    }

    private GroupJoinEmojiChainEvent(int resultCode) : base(resultCode) { }
    
    public static GroupJoinEmojiChainEvent Create(uint targetMessageSeq, uint targetFaceId, uint groupUin)
        => new(targetMessageSeq, targetFaceId, groupUin);
}
