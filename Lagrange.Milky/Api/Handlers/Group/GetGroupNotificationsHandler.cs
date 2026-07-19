using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Converters;
using Lagrange.Milky.Models;

namespace Lagrange.Milky.Api.Handlers.Group;

[ApiHandler("get_group_notifications")]
public sealed class GetGroupNotificationsHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetGroupNotificationsHandler.Request, GetGroupNotificationsHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var notifications = await (request.IsFiltered switch
        {
            true => _lagrange.FetchFilteredGroupNotifications(
                (ulong)request.Limit,
                (ulong?)request.StartNotificationSeq ?? 0
            ).WaitAsync(ct),
            false => _lagrange.FetchGroupNotifications(
                (ulong)request.Limit,
                (ulong?)request.StartNotificationSeq ?? 0
            ).WaitAsync(ct),
        });
        
        return new MilkyApiResponse<Result>(new Result
        {
            Notifications = [.. notifications.Select(_converter.ToGroupNotification)],
            NextNotificationSeq = null,
        });
    }

    public sealed class Request(long? startNotificationSeq, bool isFiltered = false, int limit = 20)
    {
        [JsonPropertyName("start_notification_seq")] public long? StartNotificationSeq { get; init; } = startNotificationSeq;
        [JsonPropertyName("is_filtered")] public bool IsFiltered { get; init; } = isFiltered;
        [JsonPropertyName("limit")] public int Limit { get; init; } = limit;
    }

    public sealed class Result
    {
        [JsonPropertyName("notifications")] public required IReadOnlyList<GroupNotificationBase> Notifications { get; init; }
        [JsonPropertyName("next_notification_seq")] public required long? NextNotificationSeq { get; init; }
    }
}
