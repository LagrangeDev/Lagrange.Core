using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Message;

internal class RecallPokeEventReq(bool isGroup, ulong peerUin, ulong messageSequence, ulong messageTime, ulong tipsSeqId) : ProtocolEvent
{
    public bool IsGroup { get; } = isGroup;

    public ulong PeerUin { get; } = peerUin;

    public ulong MessageSequence { get; } = messageSequence;

    public ulong MessageTime { get; } = messageTime;

    public ulong TipsSeqId { get; } = tipsSeqId;
}

internal class RecallPokeEventResp : ProtocolEvent;
