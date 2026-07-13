using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

public class BotOfflineEventData
{
    [JsonPropertyName("reason")] public required string Reason { get; init; }
}