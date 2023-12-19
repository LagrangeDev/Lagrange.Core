namespace Lagrange.Core.Internal.Event.Action;

internal class GroupRemarkEvent : ProtocolEvent
{
    public uint GroupUin { get; set; }

    public string TargetRemark { get; set; } = "";

    private GroupRemarkEvent(uint groupUin, string targetRemark) : base(true)
    {
        GroupUin = groupUin;
        TargetRemark = targetRemark;
    }

    private GroupRemarkEvent(int resultCode) : base(resultCode) { }
    
    public static GroupRemarkEvent Create(uint groupUin, string targetRemark) => new(groupUin, targetRemark);
    
    public static GroupRemarkEvent Result(int resultCode) => new(resultCode);
}