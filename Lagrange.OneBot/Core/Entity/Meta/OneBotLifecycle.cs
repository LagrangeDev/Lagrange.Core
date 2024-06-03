using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Meta;

[Serializable]
public class OneBotLifecycle(uint selfId, string subType) : OneBotMeta(selfId, "lifecycle")
{
    [JsonPropertyName("sub_type")] public string SubType { get; set; } = subType;
}