namespace Lagrange.Core.Internal.Event.Message;

internal class MarkReadedEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string? TargetUid { get; }
    
    public uint StartSequence { get; }
    
    public uint Time { get; }

    private MarkReadedEvent(uint groupUin, string? targetUid, uint startSequence, uint time) : base(true)
    {
        GroupUin = groupUin;
        TargetUid = targetUid;
        StartSequence = startSequence;
        Time = time;
    }

    private MarkReadedEvent(int resultCode) : base(resultCode) { }
    
    public static MarkReadedEvent Create(uint groupUin, string? targetUid, uint startSequence, uint time) =>
        new(groupUin, targetUid, startSequence, time);

    public static MarkReadedEvent Result(int resultCode) => new(resultCode);
}