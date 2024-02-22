namespace Lagrange.Core.Internal.Event.Notify;

internal class FriendSysRecallEvent : ProtocolEvent
{
    public string FromUid { get; }
    
    public uint Sequence { get; }
    
    public uint Time { get; }
    
    public uint Random { get; }
    
    private FriendSysRecallEvent(string fromUid, uint sequence, uint time, uint random) : base(0)
    {
        FromUid = fromUid;
        Sequence = sequence;
        Time = time;
        Random = random;
    }

    public static FriendSysRecallEvent Result(string fromUid, uint sequence, uint time, uint random)
        => new(fromUid, sequence, time, random);
}