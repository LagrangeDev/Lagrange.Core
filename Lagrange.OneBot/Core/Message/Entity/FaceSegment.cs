using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class FaceSegment(int id)
{
    public FaceSegment() : this(0) { }
    
    [JsonPropertyName("id")] [CQProperty] public string Id { get; set; } = id.ToString();
}

[SegmentSubscriber(typeof(FaceEntity), "face")]
public partial class FaceSegment : ISegment
{
    public void Build(MessageBuilder builder, ISegment segment)
    {
        if (segment is FaceSegment faceSegment) builder.Face(ushort.Parse(faceSegment.Id));
    }
    
    public ISegment FromEntity(IMessageEntity entity)
    {
        if (entity is not FaceEntity faceEntity) throw new ArgumentException("Invalid entity type.");

        return new FaceSegment(faceEntity.FaceId);
    }
}