using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupJoinRequestEventConverter : IEventConverter<BotGroupJoinNotificationEvent, GroupJoinRequestEventData>
{
    public string Name => "group_join_request";

    public bool CanConvert(BotGroupJoinNotificationEvent @event) => true;

    public ValueTask<GroupJoinRequestEventData> ConvertAsync(BotGroupJoinNotificationEvent @event, CancellationToken ct) => ValueTask.FromResult(new GroupJoinRequestEventData
    {
        GroupId = @event.Notification.GroupUin,
        NotificationSeq = (long)@event.Notification.Sequence,
        IsFiltered = @event.Notification.IsFiltered,
        InitiatorId = @event.Notification.TargetUin,
        Comment = @event.Notification.Comment,
    });
}