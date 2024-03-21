using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupMemo
{
    [JsonPropertyName("group_id")] public long GroupId { get; set; }

    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;

    [JsonPropertyName("image")] public string Image { get; set; } = string.Empty;
}
