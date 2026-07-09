using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;

namespace Lagrange.Core.Internal.Events.System;

internal class SetFilteredGroupNotificationEventReq(long groupUin, ulong sequence, BotGroupNotificationType type, GroupNotificationOperate operate, string message) : ProtocolEvent
{
    public long GroupUin { get; } = groupUin;

    public ulong Sequence { get; } = sequence;

    public BotGroupNotificationType Type { get; } = type;

    public GroupNotificationOperate Operate { get; } = operate;

    public string Message { get; } = message;
}

internal class SetFilteredGroupNotificationEventResp : ProtocolEvent
{
    public static readonly SetFilteredGroupNotificationEventResp Default = new();
}