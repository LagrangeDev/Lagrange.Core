using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Message;

internal class GroupRecallMsgEventReq(long groupUin, ulong sequence) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public ulong Sequence { get; } = sequence;
}

internal class GroupRecallMsgEventResp : ProtocolEvent;