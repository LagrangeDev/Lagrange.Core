using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupLeave
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("is_dismiss")] public bool IsDismiss { get; set; }
}