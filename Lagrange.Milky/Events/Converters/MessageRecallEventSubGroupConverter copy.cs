using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class MessageRecallEventSubFriendConverter : IEventConverter<BotFriendRecallEvent, MessageRecallEventData>
{
    public string Name => "message_recall";

    public bool CanConvert(BotFriendRecallEvent @event) => true;

    public ValueTask<MessageRecallEventData> ConvertAsync(BotFriendRecallEvent @event, CancellationToken ct) => ValueTask.FromResult(new MessageRecallEventData
    {
        MessageScene = "friend",
        PeerId = @event.PeerUin,
        MessageSeq = (long)@event.Sequence,
        SenderId = @event.AuthorUin,
        OperatorId = @event.AuthorUin,
        DisplaySuffix = @event.Tip,
    });
}