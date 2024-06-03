namespace Lagrange.Core.Internal.Event.Action;

internal class GroupFSSpaceEvent : GroupFSViewEvent
{
    public ulong TotalSpace { get; set; }
    
    public ulong UsedSpace { get; set; }
    
    private GroupFSSpaceEvent(uint groupUin) : base(groupUin) { }

    private GroupFSSpaceEvent(int resultCode, ulong totalSpace, ulong usedSpace) : base(resultCode)
    {
        TotalSpace = totalSpace;
        UsedSpace = usedSpace;
    }

    public static GroupFSSpaceEvent Create(uint groupUin) => new(groupUin);

    public static GroupFSSpaceEvent Result(int resultCode, ulong totalSpace, ulong usedSpace) =>
        new(resultCode, totalSpace, usedSpace);
}