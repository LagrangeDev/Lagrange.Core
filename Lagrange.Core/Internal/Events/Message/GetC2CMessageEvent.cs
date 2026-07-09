using Lagrange.Core.Events;
using Lagrange.Core.Internal.Packets.Message;

namespace Lagrange.Core.Internal.Events.Message;

internal class GetC2CMessageEventReq(string peerUid, ulong startSequence, ulong endSequence) : ProtocolEvent
{
    public string PeerUid { get; } = peerUid;

    public ulong StartSequence { get; } = startSequence;

    public ulong EndSequence { get; } = endSequence;
}

internal class GetC2CMessageEventResp(List<CommonMessage> chains) : ProtocolEvent
{
    public List<CommonMessage> Chains { get; } = chains;
}