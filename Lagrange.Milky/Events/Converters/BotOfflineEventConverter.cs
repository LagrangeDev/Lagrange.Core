using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Events.Attributes;

namespace Lagrange.Milky.Events.Converters;

[EventConverter]
public class BotOfflineEventConverter : IEventConverter<BotOfflineEvent, BotOfflineEventConverter.Data>
{
    public string Name => "bot_offline";

    public bool CanConvert(BotOfflineEvent @event) => true;

    public ValueTask<Data> ConvertAsync(BotOfflineEvent @event, CancellationToken ct) => ValueTask.FromResult(new Data
    {
        Reason = $"{@event.Reason} ({@event.Tips?.Tag}, {@event.Tips?.Message})",
    });

    public class Data
    {
        [JsonPropertyName("reason")] public required string Reason { get; init; }
    }
}