using System.Text.Json.Serialization;
using MessagePack;

namespace Lagrange.OneBot.Core.Entity.Generic;

[Serializable]
[MessagePackObject]
internal abstract class OneBotRequest
{
    protected OneBotRequest(string type, string detailType, string subType)
    {
        Id = Guid.NewGuid();
        Time = DateTimeOffset.Now.ToUnixTimeSeconds();
        Type = type;
        DetailType = detailType;
        SubType = subType;
    }

    [JsonPropertyName("id")] [Key("id")] public Guid Id { get; set; }
    
    [JsonPropertyName("time")] [Key("time")] public float Time { get; set; }
    
    [JsonPropertyName("type")] [Key("type")] public string Type { get; set; }
    
    [JsonPropertyName("detail_type")] [Key("detail_type")] public string DetailType { get; set; }
    
    [JsonPropertyName("sub_type")] [Key("sub_type")] public string SubType { get; set; }
}