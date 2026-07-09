using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class ReduceGroupReactionEventReq(long groupUin, ulong sequence, string code) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public ulong Sequence { get; } = sequence;

    public string Code { get; } = code;
}

internal class ReduceGroupReactionEventResp : ProtocolEvent
{
    public static readonly ReduceGroupReactionEventResp Default = new();
}