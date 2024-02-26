namespace Lagrange.Core.Event.EventArg;

public class GroupInvitationRequestEvent : EventBase
{
    internal GroupInvitationRequestEvent(uint groupUin, uint targetUin, uint invitorUin)
    {
        GroupUin = groupUin;
        TargetUin = targetUin;
        InvitorUin = invitorUin;
        EventMessage = $"[{nameof(GroupInvitationRequestEvent)}] {TargetUin} from {InvitorUin} at {GroupUin}";
    }
    
    public uint GroupUin { get; }
    
    public uint TargetUin { get; }
    
    public uint InvitorUin { get; }
}