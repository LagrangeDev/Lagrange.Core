using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGroupMessage
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("message")] public List<OneBotSegment> Messages { get; set; } = new();
}