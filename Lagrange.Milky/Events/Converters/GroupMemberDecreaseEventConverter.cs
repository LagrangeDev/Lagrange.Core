using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupMemberDecreaseEventConverter : IEventConverter<BotGroupMemberDecreaseEvent, GroupMemberDecreaseEventData>
{
    public string Name => "group_member_decrease";

    public bool CanConvert(BotGroupMemberDecreaseEvent @event) => true;

    public ValueTask<GroupMemberDecreaseEventData> ConvertAsync(BotGroupMemberDecreaseEvent @event, CancellationToken ct) => ValueTask.FromResult(new GroupMemberDecreaseEventData
    {
        GroupId = @event.GroupUin,
        UserId = @event.UserUin,
        OperatorId = @event.OperatorUin,
    });
}