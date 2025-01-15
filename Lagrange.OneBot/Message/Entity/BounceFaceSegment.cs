using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class BounceFaceSegment(uint faceid,uint count,string name)
{
    public BounceFaceSegment() : this(0,1,"") { }
    [JsonPropertyName("id")] [CQProperty] public uint FaceId { get; set; } = faceid;
    [JsonPropertyName("count")] [CQProperty] public uint Count { get; set; } = count;
    [JsonPropertyName("name")] [CQProperty] public string Name { get; set; } = name;
}

[SegmentSubscriber(typeof(BounceFaceSegment), "bounceface")]
public partial class BounceFaceSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is BounceFaceSegment bounceFace) builder.Add(new BounceFaceEntity(bounceFace.FaceId, bounceFace.Count, bounceFace.Name));
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not BounceFaceEntity bounceFace) throw new ArgumentException("Invalid entity type.");

        return new BounceFaceSegment(bounceFace.FaceId, bounceFace.Count, bounceFace.Name);
    }
}