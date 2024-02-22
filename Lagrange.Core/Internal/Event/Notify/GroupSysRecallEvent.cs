namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysRecallEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string Uid { get; }
    
    public uint Sequence { get; }
    
    public uint Time { get; }
    
    public uint Random { get; }

    private GroupSysRecallEvent(uint groupUin, string uid, uint sequence, uint time, uint random) : base(0)
    {
        GroupUin = groupUin;
        Uid = uid;
        Sequence = sequence;
        Time = time;
        Random = random;
    }

    public static GroupSysRecallEvent Create(uint groupUin, string uid, uint sequence, uint time, uint random) 
        => new(groupUin, uid, sequence, time, random);
}