using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupWholeBan
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("enable")] public bool Enable { get; set; }
}