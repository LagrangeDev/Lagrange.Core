using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotUploadPrivateFile
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("file")] public string File { get; set; } = string.Empty;

    [JsonPropertyName("name")] public string? Name { get; set; }
}