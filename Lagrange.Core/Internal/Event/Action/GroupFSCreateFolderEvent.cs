namespace Lagrange.Core.Internal.Event.Action;

internal class GroupFSCreateFolderEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string Name { get; } = string.Empty;

    private GroupFSCreateFolderEvent(uint groupUin, string name) : base(true)
    {
        GroupUin = groupUin;
        Name = name;
    }

    private GroupFSCreateFolderEvent(int resultCode) : base(resultCode) { }
    
    public static GroupFSCreateFolderEvent Create(uint groupUin, string name) => new(groupUin, name);
    
    public static GroupFSCreateFolderEvent Result(int resultCode) => new(resultCode);
}