using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;

namespace Lagrange.Milky.Events.Converters;

[EventConverter(Priority = -1)]
public class GroupInvitationEventConverter(BotContext lagrange) : IEventConverter<BotGroupInviteNotificationEvent, GroupInvitationEventConverter.Data>
{
    private readonly BotContext _lagrange = lagrange;

    public string Name => "group_invitation";

    public bool CanConvert(BotGroupInviteNotificationEvent @event) => @event.Notification.TargetUin == _lagrange.BotUin;

    public ValueTask<Data> ConvertAsync(BotGroupInviteNotificationEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
    {
        GroupId = @event.Notification.GroupUin,
        InvitationSeq = (long)@event.Notification.Sequence,
        InitiatorId = @event.Notification.InviterUin,
        TargetUserId = @event.Notification.TargetUin,
    });

    public class Data
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; }
        [JsonPropertyName("invitation_seq")] public required long InvitationSeq { get; init; }
        [JsonPropertyName("initiator_id")] public required long InitiatorId { get; init; }
        [JsonPropertyName("source_group_id")] public required long? TargetUserId { get; init; }
    }
}