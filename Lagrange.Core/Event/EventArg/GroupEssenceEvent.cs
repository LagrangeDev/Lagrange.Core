namespace Lagrange.Core.Event.EventArg;

public class GroupEssenceEvent : EventBase
{
    public uint GroupUin { get; }

    public uint OperatorUin { get; }

    public uint AuthorUin { get; }

    public uint Sequence { get; }

    public uint SubType { get; }

    public GroupEssenceEvent(uint groupUin, uint operatorUin, uint authorUin, uint sequence, uint subType)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        AuthorUin = authorUin;
        Sequence = sequence;
        SubType = subType;

        EventMessage = $"{nameof(GroupPokeEvent)}: {GroupUin} | {OperatorUin} | {authorUin} | {sequence} | {subType}";
    }
}