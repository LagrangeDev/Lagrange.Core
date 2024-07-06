using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupBot
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("bot_id")] public uint BotId { get; set; }
    
    [JsonPropertyName("enable")] public uint Enable { get; set; }
}