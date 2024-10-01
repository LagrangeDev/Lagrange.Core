using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupReaction
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("message_id")] public int MessageId { get; set; }

    [JsonPropertyName("code")] public required string Code { get; set; }

    [JsonPropertyName("is_add")] public bool IsAdd { get; set; } = true;
}