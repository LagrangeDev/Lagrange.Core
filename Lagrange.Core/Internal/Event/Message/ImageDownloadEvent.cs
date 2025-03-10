using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

internal class ImageDownloadEvent : MediaDownloadEvent
{
    protected ImageDownloadEvent(IndexNode index) : base(index) { }

    protected ImageDownloadEvent(int code, string message, string url) : base(code, message, url) { }

    public static new ImageDownloadEvent Create(IndexNode index) => new(index);

    public static new ImageDownloadEvent Result(string url) => new(0, string.Empty, url);
    public static new ImageDownloadEvent Result(int code, string message) => new(code, message, string.Empty);
}