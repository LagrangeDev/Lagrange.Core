using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public abstract class OneBotEntityBase
{
    [JsonPropertyName("time")] public long Time { get; set; }
    
    [JsonPropertyName("self_id")] public uint SelfId { get; set; }
    
    [JsonPropertyName("post_type")] public string PostType { get; set; }
    
    protected OneBotEntityBase(uint selfId, string postType)
    {
        Time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        SelfId = selfId;
        PostType = postType;
    }
}