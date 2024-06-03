namespace Lagrange.Core.Internal.Event.Notify;

internal class GroupSysAdminEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string Uid { get; }
    
    public bool IsPromoted { get; }
    
    private GroupSysAdminEvent(uint groupUin, string uid, bool isPromoted) : base(0)
    {
        GroupUin = groupUin;
        Uid = uid;
        IsPromoted = isPromoted;
    }
    
    public static GroupSysAdminEvent Result(uint groupUin, string uid, bool isPromoted) => new(groupUin, uid, isPromoted);
}