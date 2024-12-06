using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetPrivateFile
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("file_hash")] public string FileHash { get; set; } = string.Empty;
    
    [JsonPropertyName("file_id")] public required string FileId { get; set; }
}