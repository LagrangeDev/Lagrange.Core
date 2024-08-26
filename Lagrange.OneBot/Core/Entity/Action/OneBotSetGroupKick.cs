using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupKick
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("reject_add_request")] public bool RejectAddRequest { get; set; }
}