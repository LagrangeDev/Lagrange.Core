using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupAdmin
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("enable")] public bool Enable { get; set; }
}