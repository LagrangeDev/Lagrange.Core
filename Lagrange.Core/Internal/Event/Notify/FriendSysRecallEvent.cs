namespace Lagrange.Core.Internal.Event.Notify;

internal class FriendSysRecallEvent : ProtocolEvent
{
    public string FromUid { get; }

    public uint Sequence { get; }

    public uint Time { get; }

    public uint Random { get; }

    public string Tip { get; }

    private FriendSysRecallEvent(string fromUid, uint sequence, uint time, uint random, string tip) : base(0)
    {
        FromUid = fromUid;
        Sequence = sequence;
        Time = time;
        Random = random;
        Tip = tip;
    }

    public static FriendSysRecallEvent Result(string fromUid, uint sequence, uint time, uint random, string tip)
        => new(fromUid, sequence, time, random, tip);
}