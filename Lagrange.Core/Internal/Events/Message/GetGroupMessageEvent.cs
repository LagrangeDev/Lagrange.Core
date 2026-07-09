using Lagrange.Core.Events;
using Lagrange.Core.Internal.Packets.Message;

namespace Lagrange.Core.Internal.Events.Message;

internal class GetGroupMessageEventReq(long groupUin, ulong startSequence, ulong endSequence) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public ulong StartSequence { get; } = startSequence;

    public ulong EndSequence { get; } = endSequence;
}

internal class GetGroupMessageEventResp(List<CommonMessage> chains) : ProtocolEvent
{
    public List<CommonMessage> Chains { get; } = chains;
}