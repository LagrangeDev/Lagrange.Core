using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Common;
#pragma warning disable CS8618

[Serializable]
public class Pic
{
    [JsonPropertyName("back")] public bool Back { get; set; }

    [JsonPropertyName("desc")] public string Desc { get; set; }

    [JsonPropertyName("jumpUrl")] public string JumpUrl { get; set; }

    [JsonPropertyName("preview")] public string Preview { get; set; }

    [JsonPropertyName("tag")] public string Tag { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; }
}