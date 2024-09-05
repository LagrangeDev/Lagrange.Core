using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotImageExpires
{
    [JsonPropertyName("url")] public string? Url { get; set; }
    
    [JsonPropertyName("expires")] public string? Expires { get; set; }

    
}