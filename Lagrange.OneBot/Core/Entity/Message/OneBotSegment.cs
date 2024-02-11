using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Message;
using Lagrange.OneBot.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Entity.Message;

public class OneBotSegment(string type, SegmentBase data)
{
    public OneBotSegment() : this("", new TextSegment()) { }

    [JsonPropertyName("type")] public string Type { get; set; } = type;

    [JsonPropertyName("data")] public object Data { get; set; } = data;
}