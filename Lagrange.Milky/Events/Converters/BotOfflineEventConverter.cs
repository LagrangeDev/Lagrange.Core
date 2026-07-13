using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;
using Lagrange.Milky.Models.Events;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class BotOfflineEventConverter : IEventConverter<BotOfflineEvent, BotOfflineEventData>
{
    public string Name => "bot_offline";

    public bool CanConvert(BotOfflineEvent @event) => true;

    public ValueTask<BotOfflineEventData> ConvertAsync(BotOfflineEvent @event, CancellationToken ct) => ValueTask.FromResult(new BotOfflineEventData
    {
        Reason = $"{@event.Reason} ({@event.Tips?.Tag}, {@event.Tips?.Message})",
    });
}