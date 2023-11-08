using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetForwardMsg
{
    [JsonPropertyName("id")] public string Id { get; set; } = ""; // resId
}