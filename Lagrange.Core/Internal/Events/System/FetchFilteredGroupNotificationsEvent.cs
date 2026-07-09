using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class FetchFilteredGroupNotificationsEventReq(ulong count, ulong start = 0) : ProtocolEvent
{
    public ulong Count { get; } = count;

    public ulong Start { get; } = start;
}

internal class FetchFilteredGroupNotificationsEventResp(List<BotGroupNotificationBase> groupNotifications) : ProtocolEvent
{
    public List<BotGroupNotificationBase> GroupNotifications { get; } = groupNotifications;
}