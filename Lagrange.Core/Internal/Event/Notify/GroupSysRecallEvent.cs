namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysRecallEvent : ProtocolEvent
{
    public uint GroupUin { get; }

    public string AuthorUid { get; }

    public string? OperatorUid { get; }

    public uint Sequence { get; }

    public uint Time { get; }

    public uint Random { get; }

    public string Tip { get; }

    private GroupSysRecallEvent(uint groupUin, string authorUid, string? operatorUid, uint sequence, uint time, uint random, string tip) : base(0)
    {
        GroupUin = groupUin;
        AuthorUid = authorUid;
        OperatorUid = operatorUid;
        Sequence = sequence;
        Time = time;
        Random = random;
        Tip = tip;
    }

    public static GroupSysRecallEvent Result(uint groupUin, string authorUid, string? operatorUid, uint sequence, uint time, uint random, string tip)
        => new(groupUin, authorUid, operatorUid, sequence, time, random, tip);
}