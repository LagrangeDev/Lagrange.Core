using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotGetMusicArk
{
    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
    
    [JsonPropertyName("desc")] public string Desc { get; set; } = string.Empty;
    
    [JsonPropertyName("jumpUrl")] public string JumpUrl { get; set; } = string.Empty;
    
    [JsonPropertyName("musicUrl")] public string MusicUrl { get; set; } = string.Empty;
    
    [JsonPropertyName("source_icon")] public string SourceIcon { get; set; } = string.Empty;
    
    [JsonPropertyName("tag")] public string Tag { get; set; } = string.Empty;
    
    [JsonPropertyName("preview")] public string Preview { get; set; } = string.Empty;
    
    [JsonPropertyName("sourceMsgId")] public string SourceMsgId { get; set; } = string.Empty;
}