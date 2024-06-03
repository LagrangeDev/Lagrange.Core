using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetRequest
{
    [JsonPropertyName("flag")] public string Flag { get; set; } = string.Empty;

    [JsonPropertyName("approve")] public bool Approve { get; set; } = true;
}