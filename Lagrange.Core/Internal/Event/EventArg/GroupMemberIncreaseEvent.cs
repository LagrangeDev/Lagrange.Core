namespace Lagrange.Core.Internal.Event.EventArg;

public class GroupMemberIncreaseEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint MemberUin { get; }
    
    public uint? InvitorUin { get; }
    
    public GroupMemberIncreaseEvent(uint groupUin, uint memberUin, uint? invitorUin)
    {
        GroupUin = groupUin;
        MemberUin = memberUin;
        InvitorUin = invitorUin;
        
        EventMessage = $"{nameof(GroupMemberIncreaseEvent)}: {GroupUin} | {MemberUin} | {InvitorUin}";
    }
}