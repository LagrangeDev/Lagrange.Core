using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupJoinRequestEventConverter : IEventConverter<BotGroupJoinNotificationEvent, GroupJoinRequestEventConverter.Data>
{
    public string Name => "group_join_request";

    public bool CanConvert(BotGroupJoinNotificationEvent @event) => true;

    public ValueTask<Data> ConvertAsync(BotGroupJoinNotificationEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
    {
        GroupId = @event.Notification.GroupUin,
        NotificationSeq = (long)@event.Notification.Sequence,
        IsFiltered = @event.Notification.IsFiltered,
        InitiatorId = @event.Notification.TargetUin,
        Comment = @event.Notification.Comment,
    });

    public class Data
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; }
        [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
        [JsonPropertyName("is_filtered")] public required bool IsFiltered { get; init; }
        [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
        [JsonPropertyName("comment")] public required string Comment { get; init; }
    }
}