using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class AtSegment(uint at, string? name)
{
    public AtSegment() : this(0, null) { }
    
    public AtSegment(uint at) : this(at, null) { }
    
    [JsonPropertyName("qq")] [CQProperty] public string At { get; set; } = at == 0 ? "all" : at.ToString();
    
    [JsonPropertyName("name")] [CQProperty] public string? Name { get; set; } = name;
}

[SegmentSubscriber(typeof(MentionEntity), "at")]
public partial class AtSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is AtSegment atSegment) builder.Mention(atSegment.At == "all" ? 0 : uint.Parse(atSegment.At), atSegment.At == "all" ? "@\u5168\u4f53\u6210\u5458" : null);
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not MentionEntity mentionEntity) throw new ArgumentException("Invalid entity type.");

        return new AtSegment(mentionEntity.Uin, mentionEntity.Name);
    }
}