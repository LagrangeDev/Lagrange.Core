using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupInvitedJoinRequestEventConverter(BotContext lagrange) : IEventConverter<BotGroupInviteNotificationEvent, GroupInvitedJoinRequestEventData>
{
    private readonly BotContext _lagrange = lagrange;

    public string Name => "group_invited_join_request";

    public bool CanConvert(BotGroupInviteNotificationEvent @event) => @event.Notification.TargetUin != _lagrange.BotUin;

    public ValueTask<GroupInvitedJoinRequestEventData> ConvertAsync(BotGroupInviteNotificationEvent @event, CancellationToken ct) => ValueTask.FromResult(new GroupInvitedJoinRequestEventData
    {
        GroupId = @event.Notification.GroupUin,
        NotificationSeq = (long)@event.Notification.Sequence,
        InitiatorId = @event.Notification.InviterUin,
        TargetUserId = @event.Notification.TargetUin,
    });
}