using Lagrange.Core.Common.Response;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class GroupGetTodoEventReq(long groupUin) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;
}

internal class GroupGetTodoEventResp(BotGetGroupTodoResult result) : ProtocolEvent
{
    public BotGetGroupTodoResult Result { get; } = result;
}
