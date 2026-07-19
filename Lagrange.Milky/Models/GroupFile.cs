using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models;

public sealed class GroupFile {
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("file_id")] public required string FileId { get; init; }
    [JsonPropertyName("file_name")] public required string FileName { get; init; }
    [JsonPropertyName("parent_folder_id")] public required string ParentFolderId { get; init; }
    [JsonPropertyName("file_size")] public required long FileSize { get; init; }
    [JsonPropertyName("uploaded_time")] public required long UploadedTime { get; init; }
    [JsonPropertyName("expire_time")] public required long? ExpireTime { get; init; }
    [JsonPropertyName("uploader_id")] public required long UploaderId { get; init; }
    [JsonPropertyName("downloaded_times")] public required int DownloadedTimes { get; init; }
}