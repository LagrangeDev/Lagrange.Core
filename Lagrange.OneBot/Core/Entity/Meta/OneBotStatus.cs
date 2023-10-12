using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Meta;

[Serializable]
public class OneBotStatus(bool online, bool good)
{
    [JsonPropertyName("app_initialized")] public bool AppInitialized { get; set; } = true;

    [JsonPropertyName("app_enabled")] public bool AppEnabled { get; set; } = true;

    [JsonPropertyName("app_good")] public bool AppGood { get; set; } = true;

    [JsonPropertyName("online")] public bool Online { get; set; } = online;

    [JsonPropertyName("good")] public bool Good { get; set; } = good;
}