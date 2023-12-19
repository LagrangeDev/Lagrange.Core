namespace Lagrange.Core.Event.EventArg;

public class GroupAdminChangedEvent : EventBase
{
    public uint GroupUin { get; }
    
    public uint AdminUin { get; }
    
    public bool IsPromote { get; }
    
    public GroupAdminChangedEvent(uint groupUin, uint adminUin, bool isPromote)
    {
        GroupUin = groupUin;
        AdminUin = adminUin;
        IsPromote = isPromote;
        EventMessage = $"{nameof(GroupAdminChangedEvent)} | {GroupUin} | {AdminUin} | {IsPromote}";
    }
}