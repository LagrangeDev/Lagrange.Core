using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotResult(object? data, int retCode, string status)
{
    [JsonPropertyName("status")] public string Status { get; set; } = status;

    [JsonPropertyName("retcode")] public int RetCode { get; set; } = retCode;

    [JsonPropertyName("data")] public object? Data { get; set; } = data;

    [JsonPropertyName("echo")] public object? Echo { get; set; }

    [JsonIgnore] public string? Identifier { get; internal set; }
}
