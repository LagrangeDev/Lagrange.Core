using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.File;

[Serializable]
public class OneBotFolder
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("folder_id")] public string FolderId { get; set; } = string.Empty;

    [JsonPropertyName("folder_name")] public string FolderName { get; set; } = string.Empty;
    
    [JsonPropertyName("create_time")] public uint CreateTime { get; set; }
    
    [JsonPropertyName("creator")] public uint Creator { get; set; }
    
    [JsonPropertyName("create_name")] public string CreatorName { get; set; } = string.Empty;
    
    [JsonPropertyName("total_file_count")] public uint TotalFileCount { get; set; }
}