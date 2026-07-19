using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Group;

[ApiHandler("accept_group_request")]
public sealed class AcceptGroupRequestHandler(BotContext lagrange) : INoResultApiHandler<AcceptGroupRequestHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.SetGroupNotification(
            request.GroupId,
            (ulong)request.NotificationSeq,
            request.NotificationType switch
            {
                "join_request" => BotGroupNotificationType.Join,
                "invited_join_request" => BotGroupNotificationType.Invite,
                _ => throw new NotSupportedException(),
            },
            request.IsFiltered,
            GroupNotificationOperate.Allow
        ).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long notificationSeq, string notificationType, long groupId, bool isFiltered = false)
    {
        [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; } = notificationSeq;
        [JsonPropertyName("notification_type")] public required string NotificationType { get; init; } = notificationType;
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("is_filtered")] public bool IsFiltered { get; init; } = isFiltered;
    }
}
