using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class AtSegment(uint at)
{
    public AtSegment() : this(0) { }
    
    [JsonPropertyName("qq")] public string At { get; set; } = at.ToString();
}

[SegmentSubscriber(typeof(MentionEntity), "at")]
public partial class AtSegment : ISegment
{
    public IMessageEntity ToEntity() => new MentionEntity(null, uint.Parse(At));
    
    public void Build(MessageBuilder builder, ISegment segment)
    {
        if (segment is AtSegment atSegment) builder.Mention(atSegment.At == "all" ? 0 : uint.Parse(atSegment.At), atSegment.At == "all" ? "@\u5168\u4f53\u6210\u5458" : null);
    }

    public ISegment FromEntity(IMessageEntity entity)
    {
        if (entity is not MentionEntity mentionEntity) throw new ArgumentException("Invalid entity type.");
        
        return new AtSegment(mentionEntity.Uin);
    }
}