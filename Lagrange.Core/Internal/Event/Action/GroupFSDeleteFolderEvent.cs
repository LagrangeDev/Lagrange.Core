namespace Lagrange.Core.Internal.Event.Action;

internal class GroupFSDeleteFolderEvent : ProtocolEvent
{
    public uint GroupUin { get; }
    
    public string FolderId { get; } = string.Empty;
    
    public string RetMsg { get; set; } = string.Empty;

    private GroupFSDeleteFolderEvent(uint groupUin, string folderId) : base(true)
    {
        GroupUin = groupUin;
        FolderId = folderId;
    }

    private GroupFSDeleteFolderEvent(int resultCode, string retMsg) : base(resultCode)
    {
        RetMsg = retMsg;
    }
    
    public static GroupFSDeleteFolderEvent Create(uint groupUin, string folderId) => new(groupUin, folderId);
    
    public static GroupFSDeleteFolderEvent Result(int resultCode, string retMsg) => new(resultCode, retMsg);
}