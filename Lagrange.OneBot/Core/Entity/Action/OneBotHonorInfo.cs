using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

public class OneBotHonorInfo
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
}