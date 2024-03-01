using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotRequest(uint selfId, string requestType, string? comment, string flag) : OneBotEntityBase(selfId, "request")
{
    [JsonPropertyName("request_type")] public string RequestType { get; set; } = requestType;
    
    [JsonPropertyName("comment")] public string? Comment { get; set; } = comment;
    
    [JsonPropertyName("flag")] public string Flag { get; set; } = flag;
}