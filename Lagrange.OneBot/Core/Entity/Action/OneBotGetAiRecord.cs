using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetAiRecord
{
    [JsonPropertyName("character")] public required string Character { get; set; }

    [JsonPropertyName("group_id")] public required uint GroupId { get; set; }

    [JsonPropertyName("text")] public required string Text { get; set; }

    [JsonPropertyName("chat_type")] public uint ChatType { get; set; } = 1;
}