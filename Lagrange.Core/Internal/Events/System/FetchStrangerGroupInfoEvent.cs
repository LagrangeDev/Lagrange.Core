using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class FetchStrangerGroupInfoEventReq(ulong groupUin) : ProtocolEvent
{
    public ulong GroupUin { get; } = groupUin;
}

internal class FetchStrangerGroupInfoEventResp(BotStrangerGroupInfo info) : ProtocolEvent
{
    public BotStrangerGroupInfo Info { get; } = info;
}
