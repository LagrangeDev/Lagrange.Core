namespace Lagrange.Core.Internal.Event.Message;

#pragma warning disable CS8618

internal class RecordDownloadEvent : ProtocolEvent
{
    public string AudioUrl { get; }
    
    public string Uuid { get; }
    
    public string SelfUid { get; }
    
    public string FileName { get; }
    
    public string? FileSha1 { get; }
    
    public bool IsGroup { get; }

    private RecordDownloadEvent(string uuid, string selfUid, string fileName, string? fileSha1, bool isGroup) : base(true)
    {
        Uuid = uuid;
        SelfUid = selfUid;
        FileName = fileName;
        FileSha1 = fileSha1;
        IsGroup = isGroup;
    }

    private RecordDownloadEvent(int resultCode, string audioUrl) : base(resultCode)
    {
        AudioUrl = audioUrl;
    }

    public static RecordDownloadEvent Create(string uuid, string selfUid, string fileName, string? fileSha1 = null, bool isGroup = false) 
        => new(uuid, selfUid, fileName, fileSha1, isGroup);

    public static RecordDownloadEvent Result(int resultCode, string url) => new(resultCode, url);
}