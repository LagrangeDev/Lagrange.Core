using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

internal class RecordGroupDownloadEvent : RecordDownloadEvent
{
    public uint GroupUin { get; }
    
    private RecordGroupDownloadEvent(uint groupUin, MsgInfo info) : base("", info)
    {
        GroupUin = groupUin;
    }

    private RecordGroupDownloadEvent(int resultCode, string audioUrl) : base(resultCode, audioUrl) { }
    
    public static RecordGroupDownloadEvent Create(uint groupUin, MsgInfo info) => new(groupUin, info);

    public new static RecordGroupDownloadEvent Result(int resultCode, string url) => new(resultCode, url);
}