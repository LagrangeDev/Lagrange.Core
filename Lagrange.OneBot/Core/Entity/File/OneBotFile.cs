using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.File;

[Serializable]
public class OneBotFile
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("file_id")] public string FileId { get; set; } = string.Empty;

    [JsonPropertyName("file_name")] public string FileName { get; set; } = string.Empty;
    
    [JsonPropertyName("busid")] public int BusId { get; set; }
    
    [JsonPropertyName("file_size")] public ulong FileSize { get; set; }
    
    [JsonPropertyName("upload_time")] public uint UploadTime { get; set; }
    
    [JsonPropertyName("dead_time")] public uint DeadTime { get; set; }
    
    [JsonPropertyName("modify_time")] public uint ModifyTime { get; set; }
    
    [JsonPropertyName("download_times")] public uint DownloadTimes { get; set; }
    
    [JsonPropertyName("uploader")] public uint Uploader { get; set; }

    [JsonPropertyName("uploader_name")] public string UploaderName { get; set; } = string.Empty;
}