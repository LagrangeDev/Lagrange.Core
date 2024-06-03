using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotAction
{
    [JsonPropertyName("action")] public string Action { get; set; } = "";
    
    [JsonPropertyName("params")] public JsonNode? Params { get; set; }

    [JsonPropertyName("echo")] public object? Echo { get; set; } 
}