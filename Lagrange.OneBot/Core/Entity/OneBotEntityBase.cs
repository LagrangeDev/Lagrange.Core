using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public abstract class OneBotEntityBase(uint selfId, string postType)
{
    [JsonPropertyName("time")] public long Time { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

    [JsonPropertyName("self_id")] public uint SelfId { get; set; } = selfId;

    [JsonPropertyName("post_type")] public string PostType { get; set; } = postType;
}