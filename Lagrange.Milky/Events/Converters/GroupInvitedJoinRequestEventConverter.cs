using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupInvitedJoinRequestEventConverter(BotContext lagrange) : IEventConverter<BotGroupInviteNotificationEvent, GroupInvitedJoinRequestEventConverter.Data>
{
    private readonly BotContext _lagrange = lagrange;

    public string Name => "group_invited_join_request";

    public bool CanConvert(BotGroupInviteNotificationEvent @event) => @event.Notification.TargetUin != _lagrange.BotUin;

    public ValueTask<Data> ConvertAsync(BotGroupInviteNotificationEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
    {
        GroupId = @event.Notification.GroupUin,
        NotificationSeq = (long)@event.Notification.Sequence,
        InitiatorId = @event.Notification.InviterUin,
        TargetUserId = @event.Notification.TargetUin,
    });

    public class Data
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; }
        [JsonPropertyName("notification_seq")] public required long NotificationSeq { get; init; }
        [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
        [JsonPropertyName("target_user_id")] public required long TargetUserId { get; init; }
    }
}