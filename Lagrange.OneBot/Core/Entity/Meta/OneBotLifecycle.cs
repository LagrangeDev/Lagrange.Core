using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Meta;

[Serializable]
public class OneBotLifecycle : OneBotMeta
{
    [JsonPropertyName("sub_type")] public string SubType { get; set; }

    public OneBotLifecycle(uint selfId, string subType) : base(selfId, "lifecycle")
    {
        SubType = subType;
    }
}