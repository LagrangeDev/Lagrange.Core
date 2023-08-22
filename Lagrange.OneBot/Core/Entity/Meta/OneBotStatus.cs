using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Meta;

[Serializable]
public class OneBotStatus
{
    [JsonPropertyName("app_initialized")] public bool AppInitialized { get; set; }
    
    [JsonPropertyName("app_enabled")] public bool AppEnabled { get; set; }
    
    [JsonPropertyName("app_good")] public bool AppGood { get; set; }
    
    [JsonPropertyName("online")] public bool Online { get; set; }
    
    [JsonPropertyName("good")] public bool Good { get; set; }
    
    public OneBotStatus(bool online, bool good)
    {
        AppInitialized = true;
        AppEnabled = true;
        AppGood = true;
        Online = online;
        Good = good;
    }
}