namespace Lagrange.Core.Event.EventArg;

public class GroupReactMessageWithEmojiEvent : EventBase
{
    public uint GroupUin { get; }

    public uint Sequence { get; }

    public uint FaceId { get; }

    public bool IsSet { get; }

    public GroupReactMessageWithEmojiEvent(uint groupUin, uint sequence, uint faceId, bool isSet)
    {
        GroupUin = groupUin;
        Sequence = sequence;
        FaceId = faceId;
        IsSet = isSet;

        EventMessage = $"{nameof(GroupReactMessageWithEmojiEvent)}: {GroupUin} | {Sequence} | {FaceId} | {IsSet}";
    }
}