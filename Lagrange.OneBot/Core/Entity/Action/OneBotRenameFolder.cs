using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotRenameFolder
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("folder_id")] public string FolderId { get; set; } = string.Empty;
    
    [JsonPropertyName("new_folder_name")] public string NewFolderName { get; set; } = string.Empty;
}