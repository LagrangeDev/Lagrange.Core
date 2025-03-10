using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

internal class VideoDownloadEvent : MediaDownloadEvent
{
    protected VideoDownloadEvent(IndexNode index) : base(index) { }

    protected VideoDownloadEvent(int code, string message, string url) : base(code, message, url) { }

    public static new VideoDownloadEvent Create(IndexNode index) => new(index);

    public static new VideoDownloadEvent Result(string url) => new(0, string.Empty, url);
    public static new VideoDownloadEvent Result(int code, string message) => new(code, message, string.Empty);
}