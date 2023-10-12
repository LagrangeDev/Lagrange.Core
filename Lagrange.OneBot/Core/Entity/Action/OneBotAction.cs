using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotAction<T>
{
    [JsonPropertyName("action")] public string Action { get; set; } = "";
    
    [JsonPropertyName("params")] public T? Params { get; set; }
}