namespace Lagrange.Core.Internal.Event.Message;

#pragma warning disable CS8618

internal class VideoDownloadEvent : ProtocolEvent
{
    public string AudioUrl { get; }
    
    public string Uuid { get; }
    
    public string SelfUid { get; }
    
    public string FileName { get; }
    
    public string FileMd5 { get; }
    
    public string? FileSha1 { get; }
    
    public bool IsGroup { get; }

    private VideoDownloadEvent(string uuid, string selfUid, string fileName, string fileMd5, string? fileSha1, bool isGroup) : base(true)
    {
        Uuid = uuid;
        SelfUid = selfUid;
        FileName = fileName;
        FileSha1 = fileSha1;
        IsGroup = isGroup;
        FileMd5 = fileMd5;
    }

    private VideoDownloadEvent(int resultCode, string audioUrl) : base(resultCode)
    {
        AudioUrl = audioUrl;
    }

    public static VideoDownloadEvent Create(string uuid, string selfUid, string fileName, string fileMd5, string? fileSha1 = null, bool isGroup = false) 
        => new(uuid, selfUid, fileName, fileMd5, fileSha1, isGroup);

    public static VideoDownloadEvent Result(int resultCode, string url) => new(resultCode, url);
}