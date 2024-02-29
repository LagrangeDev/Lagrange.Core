using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

internal class ImageGroupDownloadEvent : ImageDownloadEvent
{
    public uint GroupUin { get; }
    
    private ImageGroupDownloadEvent(uint groupUin, MsgInfo info) : base("", info)
    {
        GroupUin = groupUin;
    }

    private ImageGroupDownloadEvent(int resultCode, string imageUrl) : base(resultCode, imageUrl) { }
    
    public new static ImageGroupDownloadEvent Create(uint groupUin, MsgInfo info) => new(groupUin, info);

    public new static ImageGroupDownloadEvent Result(int resultCode, string url) => new(resultCode, url);
}