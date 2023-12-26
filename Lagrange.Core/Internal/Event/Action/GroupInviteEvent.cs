namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class GroupInviteEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public List<string> InviteUids { get; }

    private GroupInviteEvent(uint groupUin, List<string> inviteUids) : base(true)
    {
        GroupUin = groupUin;
        InviteUids = inviteUids;
    }

    private GroupInviteEvent(int resultCode) : base(resultCode) { }

    public static GroupInviteEvent Create(uint groupUin, List<string> inviteUids) => new(groupUin, inviteUids);

    public static GroupInviteEvent Result(int resultCode) => new(resultCode);
}