namespace Lagrange.Core.Event.EventArg;

public class GroupInvitationEvent : EventBase
{
    public uint GroupUin { get; }

    public uint InvitorUin { get; }

    public ulong? Sequence { get; }

    internal GroupInvitationEvent(uint groupUin, uint invitorUin)
    {
        GroupUin = groupUin;
        InvitorUin = invitorUin;
        Sequence = null;
        EventMessage = $"[{nameof(GroupInvitationEvent)}]: {GroupUin} from {InvitorUin}";
    }

    internal GroupInvitationEvent(uint groupUin, uint invitorUin, ulong? sequence) : this(groupUin, invitorUin)
    {
        Sequence = sequence;
    }
}