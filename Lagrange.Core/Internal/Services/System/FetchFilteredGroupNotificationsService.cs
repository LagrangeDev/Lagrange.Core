using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.System;

[EventSubscribe<FetchFilteredGroupNotificationsEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0x10c0_2")]
internal class FetchFilteredGroupNotificationsService : OidbService<FetchFilteredGroupNotificationsEventReq, FetchFilteredGroupNotificationsEventResp, FetchGroupNotificationsRequest, FetchGroupNotificationsResponse>
{
    protected override uint Command => 0x10c0;

    protected override uint Service => 2;

    protected override Task<FetchGroupNotificationsRequest> ProcessRequest(FetchFilteredGroupNotificationsEventReq request, BotContext context)
    {
        return Task.FromResult(new FetchGroupNotificationsRequest
        {
            Count = request.Count,
            StartSequence = request.Start
        });
    }

    protected override Task<FetchFilteredGroupNotificationsEventResp> ProcessResponse(FetchGroupNotificationsResponse response, BotContext context)
    {
        if (response.GroupNotifications == null)
        {
            return Task.FromResult(new FetchFilteredGroupNotificationsEventResp([]));
        }

        List<BotGroupNotificationBase> notifications = [];
        foreach (var request in response.GroupNotifications)
        {
            var targetUin = context.CacheContext.ResolveUin(request.Target.Uid);
            long? operatorUin = request.Operator != null
                ? context.CacheContext.ResolveUin(request.Operator.Uid)
                : null;
            long? inviterUin = request.Inviter != null
                ? context.CacheContext.ResolveUin(request.Inviter.Uid)
                : null;

            var notification = request.Type switch
            {
                1 => new BotGroupJoinNotification(
                    request.Group.GroupUin,
                    request.Sequence,
                    targetUin,
                    request.Target.Uid,
                    (BotGroupNotificationState)request.State,
                    operatorUin,
                    request.Operator?.Uid,
                    request.Comment,
                    true
                ),
                22 => new BotGroupInviteNotification(
                    request.Group.GroupUin,
                    request.Sequence,
                    targetUin,
                    request.Target.Uid,
                    (BotGroupNotificationState)request.State,
                    operatorUin,
                    request.Operator?.Uid,
                    inviterUin ?? 0,
                    request.Inviter?.Uid ?? string.Empty,
                    true
                ),
                _ => LogUnknownNotificationType(context, request.Type),
            };
            if (notification != null) notifications.Add(notification);
        }
        return Task.FromResult(new FetchFilteredGroupNotificationsEventResp(notifications));
    }

    private BotGroupNotificationBase? LogUnknownNotificationType(BotContext context, ulong type)
    {
        context.LogDebug(nameof(FetchFilteredGroupNotificationsService), "Unknown filtered notification type: {0}", null, type);
        return null;
    }
}