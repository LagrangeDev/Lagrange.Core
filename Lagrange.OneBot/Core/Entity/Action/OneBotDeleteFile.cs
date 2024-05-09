using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotDeleteFile
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("file_id")] public string FileId { get; set; } = string.Empty;
}