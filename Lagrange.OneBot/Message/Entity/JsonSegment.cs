using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class JsonSegment(string data)
{
    public JsonSegment(): this("") { }

    [JsonPropertyName("data")] [CQProperty] public string Data { get; set; } = data;
}

[SegmentSubscriber(typeof(LightAppEntity), "json")]
public partial class JsonSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is JsonSegment json) builder.LightApp(json.Data);
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not LightAppEntity lightAppEntity) throw new ArgumentException("Invalid entity type.");
        
        return new JsonSegment(lightAppEntity.Payload);
    }
}