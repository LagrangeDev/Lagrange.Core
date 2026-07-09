using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class FetchGroupExtraEventReq(long groupUin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;
}

internal class FetchGroupExtraEventResp(BotGroupExtra extra) : ProtocolEvent
{
    public BotGroupExtra Extra { get; } = extra;
}