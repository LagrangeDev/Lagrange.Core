using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class FetchGroupAtAllRemainEventReq(long groupUin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;
}

internal class FetchGroupAtAllRemainEventResp(bool canAtAll, uint remainAtAllCountForUin, uint remainAtAllCountForGroup) : ProtocolEvent
{
    public bool CanAtAll { get; } = canAtAll;

    public uint RemainAtAllCountForUin { get; } = remainAtAllCountForUin;

    public uint RemainAtAllCountForGroup { get; } = remainAtAllCountForGroup;
}
