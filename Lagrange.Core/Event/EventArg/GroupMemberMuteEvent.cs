namespace Lagrange.Core.Event.EventArg;

public class GroupMemberMuteEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint TargetUin { get; }

    public uint? OperatorUin { get; }
    
    public uint Duration { get; }
    
    public GroupMemberMuteEvent(uint groupUin, uint targetUin, uint? operatorUin, uint duration)
    {
        GroupUin = groupUin;
        TargetUin = targetUin;
        OperatorUin = operatorUin;
        Duration = duration;
        
        EventMessage = $"{nameof(GroupMemberMuteEvent)}: {GroupUin} | {TargetUin} | {OperatorUin} | {Duration}";
    }
}