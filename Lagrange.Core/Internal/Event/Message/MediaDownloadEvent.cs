using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

#pragma warning disable CS8618

internal class MediaDownloadEvent : ProtocolEvent
{
    public IndexNode Node { get; }

    public string Message { get; }
    public string Url { get; }

    protected MediaDownloadEvent(IndexNode index) : base(true)
    {
        Node = index;
    }

    protected MediaDownloadEvent(int code, string message, string imageUrl) : base(code)
    {
        Url = imageUrl;
    }

    public static MediaDownloadEvent Create(IndexNode index) => new(index);

    public static MediaDownloadEvent Result(string url) => new(0, string.Empty, url);
    public static MediaDownloadEvent Result(int code, string message) => new(code, message, string.Empty);
}