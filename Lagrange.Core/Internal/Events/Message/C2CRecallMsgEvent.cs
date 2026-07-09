using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Message;

internal class C2CRecallMsgEventReq(string targetUid, ulong sequence, ulong clientSequence, uint random, uint timestamp) : ProtocolEvent
{
    public string TargetUid { get; } = targetUid;
    
    public ulong Sequence { get; } = sequence;

    public ulong ClientSequence { get; } = clientSequence;

    public uint Random { get; } = random;

    public uint Timestamp { get; } = timestamp;
}

internal class C2CRecallMsgEventResp : ProtocolEvent;