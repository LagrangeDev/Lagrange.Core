using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Message;

internal class NudgeEventReq(bool isGroup, long peerUin, long targetUin) : ProtocolEvent
{
    public bool IsGroup { get; } = isGroup;
    
    public long PeerUin { get; } = peerUin;

    public long TargetUin { get; } = targetUin;
}

internal class NudgeEventResp : ProtocolEvent;