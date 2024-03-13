using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotGetStatusResponse
{
    [JsonPropertyName("app_initialized")] public bool AppInitialized { get; set; } = true;

    [JsonPropertyName("app_enabled")] public bool AppEnabled { get; set; } = true;

    [JsonPropertyName("plugins_good")] public bool PluginsGood { get; set; } = true;

    [JsonPropertyName("app_good")] public bool AppGood { get; set; } = true;

    [JsonPropertyName("online")] public bool Online { get; set; } = true;

    [JsonPropertyName("good")] public bool Good { get; set; } = true;
    
    [JsonPropertyName("memory")] public long Memory { get; set; }
}