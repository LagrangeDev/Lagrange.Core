namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class GroupInviteEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }
    
    public Dictionary<string, uint?> InviteUids { get; }

    private GroupInviteEvent(uint groupUin, Dictionary<string, uint?> inviteUids) : base(true)
    {
        GroupUin = groupUin;
        InviteUids = inviteUids;
    }

    private GroupInviteEvent(int resultCode) : base(resultCode) { }

    public static GroupInviteEvent Create(uint groupUin, Dictionary<string, uint?> inviteUids) 
        => new(groupUin, inviteUids);

    public static GroupInviteEvent Result(int resultCode) => new(resultCode);
}