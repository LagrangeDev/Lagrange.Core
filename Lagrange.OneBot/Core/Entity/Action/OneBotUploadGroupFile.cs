using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotUploadGroupFile
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("file")] public string File { get; set; } = string.Empty;

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("folder")] public string? Folder { get; set; } = string.Empty;
}