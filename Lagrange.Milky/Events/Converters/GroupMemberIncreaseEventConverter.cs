using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupMemberIncreaseEventConverter : IEventConverter<BotGroupMemberIncreaseEvent, GroupMemberIncreaseEventData>
{
    public string Name => "group_member_increase";

    public bool CanConvert(BotGroupMemberIncreaseEvent @event) => true;

    public ValueTask<GroupMemberIncreaseEventData> ConvertAsync(BotGroupMemberIncreaseEvent @event, CancellationToken ct) => ValueTask.FromResult(new GroupMemberIncreaseEventData
    {
        GroupId = @event.GroupUin,
        UserId = @event.MemberUin,
        OperatorId = @event.OperatorUin,
        InvitorId = @event.InvitorUin,
    });
}