using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Meta;

[Serializable]
public abstract class OneBotMeta : OneBotEntityBase
{
    [JsonPropertyName("meta_event_type")] public string MetaEventType { get; set; }
    
    protected OneBotMeta(uint selfId, string metaEventType) : base(selfId, "meta_event")
    {
        MetaEventType = metaEventType;
    }
}