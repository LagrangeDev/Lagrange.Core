
using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models;

public sealed class GroupFolder {
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("folder_id")] public required string FolderId { get; init; }
    [JsonPropertyName("parent_folder_id")] public required string ParentFolderId { get; init; }
    [JsonPropertyName("folder_name")] public required string FolderName { get; init; }
    [JsonPropertyName("created_time")] public required long CreatedTime { get; init; }
    [JsonPropertyName("last_modified_time")] public required long LastModifiedTime { get; init; }
    [JsonPropertyName("creator_id")] public required long CreatorId { get; init; }
    [JsonPropertyName("file_count")] public required int FileCount { get; init; }
}