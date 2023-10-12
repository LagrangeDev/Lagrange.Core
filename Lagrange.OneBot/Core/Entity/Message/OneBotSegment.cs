using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Message;

namespace Lagrange.OneBot.Core.Entity.Message;

public class OneBotSegment(string type, ISegment data)
{
    [JsonPropertyName("type")] public string Type { get; set; } = type;

    [JsonPropertyName("data")] public object Data { get; set; } = data;
}