namespace Lagrange.Core.Internal.Event.Action;

internal class GroupFSRenameFolderEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string FolderId { get; } = string.Empty;
    
    public string NewFolderName { get; } = string.Empty;
    
    public string RetMsg { get; set; } = string.Empty;

    private GroupFSRenameFolderEvent(uint groupUin, string folderId, string newFolderName) : base(true)
    {
        GroupUin = groupUin;
        FolderId = folderId;
        NewFolderName = newFolderName;
    }

    private GroupFSRenameFolderEvent(int resultCode, string retMsg) : base(resultCode)
    {
        RetMsg = retMsg;
    }
    
    public static GroupFSRenameFolderEvent Create(uint groupUin, string folderId, string newFolderName) => new(groupUin, folderId, newFolderName);
    
    public static GroupFSRenameFolderEvent Result(int resultCode, string retMsg) => new(resultCode, retMsg);
}