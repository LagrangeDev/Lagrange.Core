using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

#pragma warning disable CS8618

internal class RecordDownloadEvent : ProtocolEvent
{
    public string SelfUid { get; }
    
    public string FileUuid { get; }
    
    public IndexNode? Node { get; }
    
    public string AudioUrl { get; }
    
    protected RecordDownloadEvent(string selfUid, MsgInfo info) : base(true)
    {
        SelfUid = selfUid;
        Node = info.MsgInfoBody[0].Index;
    }

    protected RecordDownloadEvent(string selfUid, string fileUuid) : base(true)
    {
        SelfUid = selfUid;
        FileUuid = fileUuid;
    }
    
    protected RecordDownloadEvent(int resultCode, string audioUrl) : base(resultCode)
    {
        AudioUrl = audioUrl;
    }

    public static RecordDownloadEvent Create(string selfUid, MsgInfo info) => new(selfUid, info);
    
    public static RecordDownloadEvent Create(string selfUid, string fileUuid) => new(selfUid, fileUuid);

    public static RecordDownloadEvent Result(int resultCode, string url) => new(resultCode, url);
}