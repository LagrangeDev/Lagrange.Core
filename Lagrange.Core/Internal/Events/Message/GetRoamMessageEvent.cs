using Lagrange.Core.Events;
using Lagrange.Core.Internal.Packets.Message;

namespace Lagrange.Core.Internal.Events.Message;

internal class GetRoamMessageEventReq(string peerUid, uint time, uint count) : ProtocolEvent
{
    public string PeerUid { get; } = peerUid;

    public uint Time { get; } = time;

    public uint Count { get; } = count;
}

internal class GetRoamMessageEventResp(List<CommonMessage> chains) : ProtocolEvent
{
    public List<CommonMessage> Chains { get; } = chains;
}