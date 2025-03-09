using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

internal class ImageGroupDownloadEvent : MediaDownloadEvent
{
    protected ImageGroupDownloadEvent(IndexNode index) : base(index) { }

    protected ImageGroupDownloadEvent(int code, string message, string url) : base(code, message, url) { }

    public static new ImageGroupDownloadEvent Create(IndexNode index) => new(index);

    public static new ImageGroupDownloadEvent Result(string url) => new(0, string.Empty, url);
    public static new ImageGroupDownloadEvent Result(int code, string message) => new(code, message, string.Empty);
}