namespace Lagrange.Core.Internal.Event.Action;

internal class GroupSetSpecialTitleEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string TargetUid { get; } = string.Empty;

    public string Title { get; } = string.Empty;

    private GroupSetSpecialTitleEvent(uint groupUin, string targetUid, string title) : base(true)
    {
        GroupUin = groupUin;
        TargetUid = targetUid;
        Title = title;
    }

    private GroupSetSpecialTitleEvent(int resultCode) : base(resultCode) { }

    public static GroupSetSpecialTitleEvent Create(uint groupUin, string targetUid, string title)
        => new(groupUin, targetUid, title);

    public static GroupSetSpecialTitleEvent Result(int resultCode)
        => new(resultCode);
}