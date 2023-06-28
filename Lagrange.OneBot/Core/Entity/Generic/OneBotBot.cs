using System.Text.Json.Serialization;
using MessagePack;

namespace Lagrange.OneBot.Core.Entity.Generic;

[Serializable]
[MessagePackObject]
internal class OneBotBot
{
    public OneBotBot(uint uin, bool online)
    {
        Self = new OneBotSelf(uin);
        Online = online;
    }
    
    [JsonPropertyName("self")] [Key("self")] public OneBotSelf Self { get; set; }
    
    [JsonPropertyName("online")] [Key("online")] public bool Online { get; set; }
}