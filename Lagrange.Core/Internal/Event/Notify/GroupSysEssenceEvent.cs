namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysEssenceEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public uint OperatorUin { get; }

    public uint AuthorUin { get; }

    public uint Sequence { get; }

    public uint SubType { get; }

    private GroupSysEssenceEvent(uint groupUin, uint operatorUin, uint authorUin, uint sequence, uint subType) : base(0)
    {
        GroupUin = groupUin;
        OperatorUin = operatorUin;
        AuthorUin = authorUin;
        Sequence = sequence;
        SubType = subType;
    }

    public static GroupSysEssenceEvent Result(uint groupUin, uint operatorUin, uint authorUin, uint sequence, uint subType)
        => new(groupUin, operatorUin, authorUin, sequence, subType);
}