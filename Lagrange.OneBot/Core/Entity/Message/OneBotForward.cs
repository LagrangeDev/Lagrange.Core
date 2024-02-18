using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotForward
{
    [JsonPropertyName("messages")] public List<OneBotSegment> Messages { get; set; } = [];
}