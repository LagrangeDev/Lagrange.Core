using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

#pragma warning disable CS8618

internal class ImageDownloadEvent : ProtocolEvent
{
    public string SelfUid { get; }
    
    public IndexNode Node { get; }
    
    public string ImageUrl { get; }
    
    protected ImageDownloadEvent(string selfUid, MsgInfo info) : base(true)
    {
        SelfUid = selfUid;
        Node = info.MsgInfoBody[0].Index;
    }

    protected ImageDownloadEvent(int resultCode, string imageUrl) : base(resultCode)
    {
        ImageUrl = imageUrl;
    }
    
    public static ImageDownloadEvent Create(string selfUid, MsgInfo info) => new(selfUid, info);

    public static ImageDownloadEvent Result(int resultCode, string url) => new(resultCode, url);
}