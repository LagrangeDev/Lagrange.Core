namespace Lagrange.Core.Internal.Event.Action;

internal class GroupFSDownloadEvent : GroupFSOperationEvent
{
    public string FileUrl { get; set; }
    
    public string FileId { get; set; }
    
    private GroupFSDownloadEvent(uint groupUin, string fileId) : base(groupUin)
    {
        FileId = fileId;
        FileUrl = "";
    }

    private GroupFSDownloadEvent(int resultCode, string fileUrl) : base(resultCode)
    {
        FileUrl = fileUrl;
        FileId = "";
    }

    public static GroupFSDownloadEvent Create(uint groupUin, string fileId) => new(groupUin, fileId);
    
    public static GroupFSDownloadEvent Result(int resultCode, string fileId) => new(resultCode, fileId);
}