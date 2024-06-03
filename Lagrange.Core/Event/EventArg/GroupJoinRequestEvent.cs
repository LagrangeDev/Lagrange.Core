namespace Lagrange.Core.Event.EventArg;

public class GroupJoinRequestEvent : EventBase
{
    internal GroupJoinRequestEvent(uint groupUin, uint targetUin)
    {
        TargetUin = targetUin;
        GroupUin = groupUin;
        EventMessage = $"[{nameof(GroupJoinRequestEvent)}] {TargetUin} at {GroupUin}";
    }
    
    public uint TargetUin { get; }
    
    public uint GroupUin { get; }
}