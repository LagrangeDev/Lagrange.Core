using System.Text.Json.Serialization;
using Lagrange.OneBot.Core.Entity.Message;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotFakeNode
{
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("uin")] public string Uin { get; set; } = string.Empty;

    [JsonPropertyName("content")] public List<OneBotSegment> Content { get; set; } = [];
}