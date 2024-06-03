namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysMemberMuteEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string? OperatorUid { get; }
    
    public string TargetUid { get; }
    
    public uint Duration { get; }
    
    private GroupSysMemberMuteEvent(uint groupUin, string? operatorUid, string targetUid, uint duration) : base(0)
    {
        GroupUin = groupUin;
        OperatorUid = operatorUid;
        TargetUid = targetUid;
        Duration = duration;
    }
    
    public static GroupSysMemberMuteEvent Result(uint groupUin, string? operatorUid, string targetUid, uint duration) =>
        new(groupUin, operatorUid, targetUid, duration);
}