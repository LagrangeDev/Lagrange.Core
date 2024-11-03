using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetAiRecord
{
    [JsonPropertyName("character")] public string Character { get; set; } = string.Empty;

    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("text")] public string Text { get; set; } = string.Empty;

    [JsonPropertyName("chat_type")] public uint ChatType { get; set; } = 1;
}