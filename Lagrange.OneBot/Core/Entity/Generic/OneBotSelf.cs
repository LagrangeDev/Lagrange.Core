using System.Text.Json.Serialization;
using MessagePack;

namespace Lagrange.OneBot.Core.Entity.Generic;

[Serializable]
[MessagePackObject]
internal class OneBotSelf
{
    public OneBotSelf(uint userId) => UserId = userId.ToString();
    
    [JsonPropertyName("platform")] [Key("platform")] public string Platform { get; set; } = "qq";
    
    [JsonPropertyName("user_id")] [Key("user_id")] public string UserId { get; set; }
}