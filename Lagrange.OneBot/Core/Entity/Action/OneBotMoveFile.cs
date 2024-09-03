using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotMoveFile
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("file_id")] public string FileId { get; set; } = string.Empty;
    
    [JsonPropertyName("parent_directory")] public string ParentDirectory { get; set; } = string.Empty;
    
    [JsonPropertyName("target_directory")] public string TargetDirectory { get; set; } = string.Empty;
}