namespace Lagrange.Core.Internal.Event.Action;

internal class GroupSetAdminEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public string Uid { get; set; } = "";
    
    public bool IsAdmin { get; set; }

    private GroupSetAdminEvent(uint groupUin, string uid, bool isAdmin) : base(0)
    {
        GroupUin = groupUin;
        Uid = uid;
        IsAdmin = isAdmin;
    }

    private GroupSetAdminEvent(int resultCode) : base(resultCode) { }
    
    public static GroupSetAdminEvent Create(uint groupUin, string uid, bool isAdmin) => new(groupUin, uid, isAdmin);

    public static GroupSetAdminEvent Result(int resultCode) => new(resultCode);
}