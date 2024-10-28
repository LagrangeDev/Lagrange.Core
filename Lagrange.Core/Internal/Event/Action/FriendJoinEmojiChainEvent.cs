namespace Lagrange.Core.Internal.Event.Action;

internal class FriendJoinEmojiChainEvent : JoinEmojiChainEvent
{
    private FriendJoinEmojiChainEvent(uint targetMessageSeq, uint targetFaceId, string friendUid) : base(targetMessageSeq, targetFaceId)
    {
        FriendUid = friendUid;
    }

    private FriendJoinEmojiChainEvent(int resultCode) : base(resultCode) { }

    public static FriendJoinEmojiChainEvent Create(uint targetMessageSeq, uint targetFaceId, string friendUid)
        => new(targetMessageSeq, targetFaceId, friendUid);
}