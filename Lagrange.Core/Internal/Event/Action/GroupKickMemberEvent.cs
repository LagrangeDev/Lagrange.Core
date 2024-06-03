namespace Lagrange.Core.Internal.Event.Action;

internal class GroupKickMemberEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public string Uid { get; set; } = "";
    
    public bool RejectAddRequest { get; set; }

    private GroupKickMemberEvent(uint groupUin, string uid, bool rejectAddRequest) : base(0)
    {
        GroupUin = groupUin;
        Uid = uid;
        RejectAddRequest = rejectAddRequest;
    }

    private GroupKickMemberEvent(int resultCode) : base(resultCode) { }
    
    public static GroupKickMemberEvent Create(uint groupUin, string uid, bool rejectAddRequest) => new(groupUin, uid, rejectAddRequest);

    public static GroupKickMemberEvent Result(int resultCode) => new(resultCode);
}