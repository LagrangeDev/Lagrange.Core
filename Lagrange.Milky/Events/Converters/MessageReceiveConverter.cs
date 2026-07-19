using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Converters;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Messages;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public sealed class MessageReceiveConverter(MilkyConverter converter) : IEventConverter<BotMessageEvent, IncomingMessageBase>
{
    private readonly MilkyConverter _converter = converter;

    public string Name => "message_receive";

    public bool CanConvert(BotMessageEvent @event) => true;

    public async ValueTask<IncomingMessageBase> ConvertAsync(BotMessageEvent @event, CancellationToken ct)
    {
        return await _converter.ToIncomingMessageAsync(@event.Message, ct);
    }
}
