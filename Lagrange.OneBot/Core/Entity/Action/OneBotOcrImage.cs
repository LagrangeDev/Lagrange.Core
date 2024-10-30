using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotOcrImage
{
    [JsonPropertyName("image")] public string Image { get; set; } = string.Empty;
}