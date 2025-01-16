using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotGetRkeyResponse(List<OneBotRkey> rkeys)
{
    [JsonPropertyName("rkeys")] public List<OneBotRkey> Rkeys { get; set; } = rkeys;
}

[Serializable]
public class OneBotRkey
{
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("rkey")] public string? Rkey { get; set; }
    [JsonPropertyName("created_at")] public uint? CreateTime { get; set; }
    [JsonPropertyName("ttl")] public ulong? TtlSeconds { get; set; }
}