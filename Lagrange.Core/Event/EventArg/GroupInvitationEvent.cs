namespace Lagrange.Core.Event.EventArg;

public class GroupInvitationEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint InvitorUin { get; }
    
    internal GroupInvitationEvent(uint groupUin, uint invitorUin)
    {
        GroupUin = groupUin;
        InvitorUin = invitorUin;
        EventMessage = $"[{nameof(GroupInvitationEvent)}]: {GroupUin} from {InvitorUin}";
    }
}