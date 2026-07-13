using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupNudgeEventConverter : IEventConverter<BotGroupNudgeEvent, GroupNudgeEventData>
{
    public string Name => "bot_offline";

    public bool CanConvert(BotGroupNudgeEvent @event) => true;

    public ValueTask<GroupNudgeEventData> ConvertAsync(BotGroupNudgeEvent @event, CancellationToken ct) => ValueTask.FromResult(new GroupNudgeEventData
    {
        GroupId = @event.GroupUin,
        SenderId = @event.OperatorUin,
        ReceiverId = @event.TargetUin,
        DisplayAction = @event.Action,
        DisplaySuffix = @event.Suffix,
        DisplayActionImgUrl = @event.ActionImageUrl,
    });
}