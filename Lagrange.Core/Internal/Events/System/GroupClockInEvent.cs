using Lagrange.Core.Common.Response;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupClockInEventReq(long groupUin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;
}

internal class GroupClockInEventResp(BotGroupClockInResult result) : ProtocolEvent
{
    public BotGroupClockInResult Result { get; } = result;
}
