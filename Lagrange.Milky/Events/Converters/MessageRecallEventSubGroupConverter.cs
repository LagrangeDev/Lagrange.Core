using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class MessageRecallEventSubGroupConverter : IEventConverter<BotGroupRecallEvent, MessageRecallEventData>
{
    public string Name => "message_recall";

    public bool CanConvert(BotGroupRecallEvent @event) => true;

    public ValueTask<MessageRecallEventData> ConvertAsync(BotGroupRecallEvent @event, CancellationToken ct) => ValueTask.FromResult(new MessageRecallEventData
    {
        MessageScene = "group",
        PeerId = @event.GroupUin,
        MessageSeq = (long)@event.Sequence,
        SenderId = @event.AuthorUin,
        OperatorId = @event.OperatorUin,
        DisplaySuffix = @event.Tip,
    });
}