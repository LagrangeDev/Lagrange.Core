using System.Text.Json.Serialization;
using Lagrange.Core.Common.Entity;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotBusiness
{
    [JsonPropertyName("type")] public uint Type { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("level")] public uint Level { get; set; }

    [JsonPropertyName("icon")] public string? Icon { get; set; }

    [JsonPropertyName("ispro")] public uint IsPro { get; set; }

    [JsonPropertyName("isyear")] public uint IsYear { get; set; }
}
