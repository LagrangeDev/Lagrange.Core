using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupCard
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("card")] public string Card { get; set; } = "";
}