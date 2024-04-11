namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysReactMessageWithEmojiEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint Sequence { get; }

    public uint FaceId { get; }

    public bool IsSet { get; }

    private GroupSysReactMessageWithEmojiEvent(uint groupUin, uint sequence, uint faceId, bool isSet) : base(0)
    {
        GroupUin = groupUin;
        Sequence = sequence;
        FaceId = faceId;
        IsSet = isSet;
    }

    public static GroupSysReactMessageWithEmojiEvent Result(uint groupUin, uint sequence, uint faceId, bool isSet) => new(groupUin, sequence, faceId, isSet);
}