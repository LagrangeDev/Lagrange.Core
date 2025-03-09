using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

internal class RecordDownloadEvent : MediaDownloadEvent
{
    protected RecordDownloadEvent(IndexNode index) : base(index) { }

    protected RecordDownloadEvent(int code, string message, string url) : base(code, message, url) { }

    public static new RecordDownloadEvent Create(IndexNode index) => new(index);

    public static new RecordDownloadEvent Result(string url) => new(0, string.Empty, url);
    public static new RecordDownloadEvent Result(int code, string message) => new(code, message, string.Empty);
}