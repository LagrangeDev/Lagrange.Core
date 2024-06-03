namespace Lagrange.Core.Internal.Event.Action;

#pragma warning disable CS8618

internal class RenameMemberEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string TargetUid { get; }
    
    public string TargetName { get; }

    private RenameMemberEvent(uint groupUin, string targetUid, string targetName) : base(true)
    {
        GroupUin = groupUin;
        TargetUid = targetUid;
        TargetName = targetName;
    }
    
    private RenameMemberEvent(int resultCode) : base(resultCode) { }

    public static RenameMemberEvent Create(uint groupUin, string targetUid, string targetName) => new(groupUin, targetUid, targetName);

    public static RenameMemberEvent Result(int resultCode) => new(resultCode);
}