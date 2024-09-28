namespace Lagrange.Core.Internal.Event.Action;

internal class GroupKickMemberEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    public string Uid { get; set; } = "";

    public bool RejectAddRequest { get; set; }

    public string Reason { get; set; } = string.Empty;

    private GroupKickMemberEvent(uint groupUin, string uid, bool rejectAddRequest, string reason) : base(0)
    {
        GroupUin = groupUin;
        Uid = uid;
        RejectAddRequest = rejectAddRequest;
        Reason = reason;
    }

    private GroupKickMemberEvent(int resultCode) : base(resultCode) { }

    public static GroupKickMemberEvent Create(uint groupUin, string uid, bool rejectAddRequest, string reason) => new(groupUin, uid, rejectAddRequest, reason);

    public static GroupKickMemberEvent Result(int resultCode) => new(resultCode);
}