using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Meta;

[Serializable]
public abstract class OneBotMeta(uint selfId, string metaEventType) : OneBotEntityBase(selfId, "meta_event")
{
    [JsonPropertyName("meta_event_type")] public string MetaEventType { get; set; } = metaEventType;
}