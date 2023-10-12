using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotAction
{
    [JsonPropertyName("action")] public string Action { get; set; } = "";
    
    [JsonPropertyName("params")] public JsonObject? Params { get; set; }

    [JsonPropertyName("echo")] public string Echo { get; set; } = "";
}