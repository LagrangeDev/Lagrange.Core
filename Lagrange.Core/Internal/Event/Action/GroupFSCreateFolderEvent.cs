namespace Lagrange.Core.Internal.Event.Action;

internal class GroupFSCreateFolderEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string Name { get; } = string.Empty;
    
    public string RetMsg { get; set; } = string.Empty;

    private GroupFSCreateFolderEvent(uint groupUin, string name) : base(true)
    {
        GroupUin = groupUin;
        Name = name;
    }

    private GroupFSCreateFolderEvent(int resultCode, string retMsg) : base(resultCode)
    {
        RetMsg = retMsg;
    }
    
    public static GroupFSCreateFolderEvent Create(uint groupUin, string name) => new(groupUin, name);
    
    public static GroupFSCreateFolderEvent Result(int resultCode, string retMsg) => new(resultCode, retMsg);
}