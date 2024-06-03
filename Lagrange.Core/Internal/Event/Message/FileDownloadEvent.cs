namespace Lagrange.Core.Internal.Event.Message;

internal class FileDownloadEvent : ProtocolEvent
{
    public string? SenderUid { get; set; }
    
    public string? ReceiverUid { get; set; }
    
    public string? FileUuid { get; set; }
    
    public string? FileHash { get; set; }
    
    public string? FileUrl { get; set; }
    
    private FileDownloadEvent(string fileUuid, string fileHash, string? senderUid, string? receiverUid) : base(0)
    {
        FileUuid = fileUuid;
        FileHash = fileHash;
        SenderUid = senderUid;
        ReceiverUid = receiverUid;
    }

    private FileDownloadEvent(int resultCode, string fileUrl) : base(resultCode)
    {
        FileUrl = fileUrl;
    }
    
    public static FileDownloadEvent Create(string fileUuid, string fileHash, string? senderUid, string? receiverUid) => 
        new(fileUuid, fileHash, senderUid, receiverUid);

    public static FileDownloadEvent Result(int resultCode, string fileUrl) => new(resultCode, fileUrl);
}