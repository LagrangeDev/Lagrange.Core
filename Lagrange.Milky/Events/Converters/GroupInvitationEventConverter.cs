using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter(Priority = -1)]
public class GroupInvitationEventConverter(BotContext lagrange) : IEventConverter<BotGroupInviteNotificationEvent, GroupInvitationEventData>
{
    private readonly BotContext _lagrange = lagrange;

    public string Name => "group_invitation";

    public bool CanConvert(BotGroupInviteNotificationEvent @event) => @event.Notification.TargetUin == _lagrange.BotUin;

    public ValueTask<GroupInvitationEventData> ConvertAsync(BotGroupInviteNotificationEvent @event, CancellationToken ct) => ValueTask.FromResult(new GroupInvitationEventData
    {
        GroupId = @event.Notification.GroupUin,
        InvitationSeq = (long)@event.Notification.Sequence,
        InitiatorId = @event.Notification.InviterUin,
        TargetUserId = @event.Notification.TargetUin,
    });
}