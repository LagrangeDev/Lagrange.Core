using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class FetchFriendRequestsEventReq : ProtocolEvent;

internal class FetchFriendRequestsEventResp(List<BotFriendRequest> requests) : ProtocolEvent
{
    public List<BotFriendRequest> Requests { get; } = requests;
}
