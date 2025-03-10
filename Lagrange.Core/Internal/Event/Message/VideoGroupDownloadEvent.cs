using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

internal class VideoGroupDownloadEvent : MediaDownloadEvent
{
    protected VideoGroupDownloadEvent(IndexNode index) : base(index) { }

    protected VideoGroupDownloadEvent(int code, string message, string url) : base(code, message, url) { }

    public static new VideoGroupDownloadEvent Create(IndexNode index) => new(index);

    public static new VideoGroupDownloadEvent Result(string url) => new(0, string.Empty, url);
    public static new VideoGroupDownloadEvent Result(int code, string message) => new(code, message, string.Empty);
}