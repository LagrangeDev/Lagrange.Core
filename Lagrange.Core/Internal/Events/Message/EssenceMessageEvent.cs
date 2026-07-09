using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.Message;

internal class SetEssenceMessageEventReq(long groupUin, ulong sequence, uint random) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public ulong Sequence { get; } = sequence;

    public uint Random { get; } = random;
}

internal class SetEssenceMessageEventResp : ProtocolEvent
{
    public static readonly SetEssenceMessageEventResp Default = new();
}

internal class RemoveEssenceMessageEventReq(long groupUin, ulong sequence, uint random) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public ulong Sequence { get; } = sequence;

    public uint Random { get; } = random;
}

internal class RemoveEssenceMessageEventResp : ProtocolEvent
{
    public static readonly RemoveEssenceMessageEventResp Default = new();
}
