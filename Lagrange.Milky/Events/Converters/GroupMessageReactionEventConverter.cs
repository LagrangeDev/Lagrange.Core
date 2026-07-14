using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class GroupMessageReactionEventConverter : IEventConverter<BotGroupReactionEvent, GroupMessageReactionEventData>
{
    public string Name => "group_message_reaction";

    public bool CanConvert(BotGroupReactionEvent @event) => true;

    public ValueTask<GroupMessageReactionEventData> ConvertAsync(BotGroupReactionEvent @event, CancellationToken ct) => ValueTask.FromResult(new GroupMessageReactionEventData
    {
        GroupId = @event.TargetGroupUin,
        UserId = @event.OperatorUin,
        MessageSeq = (long)@event.TargetSequence,
        FaceId = @event.Code,
        ReactionType = "face", // TODO: reaction type nudeg event is not implemented in core
        IsAdd = @event.IsAdd,
    });
}