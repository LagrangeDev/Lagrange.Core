namespace Lagrange.Core.Internal.Event.Action;

internal class GroupRenameEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    public string TargetName { get; set; } = "";

    private GroupRenameEvent(uint groupUin, string targetName) : base(true)
    {
        GroupUin = groupUin;
        TargetName = targetName;
    }

    private GroupRenameEvent(int resultCode) : base(resultCode) { }
    
    public static GroupRenameEvent Create(uint groupUin, string targetName) => new(groupUin, targetName);
    
    public static GroupRenameEvent Result(int resultCode) => new(resultCode);
}