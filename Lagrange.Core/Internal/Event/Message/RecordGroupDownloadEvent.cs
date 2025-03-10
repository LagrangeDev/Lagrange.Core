using Lagrange.Core.Internal.Packets.Service.Oidb.Common;

namespace Lagrange.Core.Internal.Event.Message;

internal class RecordGroupDownloadEvent : MediaDownloadEvent
{
    protected RecordGroupDownloadEvent(IndexNode index) : base(index) { }

    protected RecordGroupDownloadEvent(int code, string message, string url) : base(code, message, url) { }

    public static new RecordGroupDownloadEvent Create(IndexNode index) => new(index);

    public static new RecordGroupDownloadEvent Result(string url) => new(0, string.Empty, url);
    public static new RecordGroupDownloadEvent Result(int code, string message) => new(code, message, string.Empty);
}