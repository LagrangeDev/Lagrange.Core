using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Message;

internal class GroupFileSendEventReq(long groupUin, string fileId, uint random) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public string FileId { get; } = fileId;

    public uint Random { get; set; } = random;
}

internal class GroupFileSendEventResp(int retCode, string? retMsg) : ProtocolEvent
{
    public int RetCode { get; } = retCode;

    public string? RetMsg { get; } = retMsg;
}