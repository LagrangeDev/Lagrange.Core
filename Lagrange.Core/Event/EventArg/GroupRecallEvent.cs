namespace Lagrange.Core.Event.EventArg;

public class GroupRecallEvent : EventBase
{
    public uint GroupUin { get; }

    public uint AuthorUin { get; }

    public uint OperatorUin { get; }

    public uint Sequence { get; }

    public uint Time { get; }

    public uint Random { get; }

    public string Tip { get; }

    public GroupRecallEvent(uint groupUin, uint authorUin, uint operatorUin, uint sequence, uint time, uint random, string tip)
    {
        GroupUin = groupUin;
        AuthorUin = authorUin;
        OperatorUin = operatorUin;
        Sequence = sequence;
        Time = time;
        Random = random;
        Tip = tip;

        EventMessage = $"{nameof(GroupRecallEvent)}: {GroupUin} | {AuthorUin} | {OperatorUin} | ({Sequence} | {Time} | {Random} | {Tip})";
    }
}