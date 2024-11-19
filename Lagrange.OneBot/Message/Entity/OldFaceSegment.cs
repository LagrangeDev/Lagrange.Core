using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class OldFaceSegment(uint id)
{
    public OldFaceSegment() : this(0) { }
    
    [JsonPropertyName("id")] [CQProperty] public string Id { get; set; } = id.ToString();
    
    [JsonPropertyName("type")] [CQProperty] public uint? Type { get; set; }
    
}

[SegmentSubscriber(typeof(OldFaceEntity), "oldface")]
public partial class OldFaceSegment : SegmentBase
{
    private static readonly uint[] Excluded = { 358, 359 };

    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is OldFaceSegment oldfaceSegment)
        {
            if (uint.TryParse(oldfaceSegment.Id, out uint faceId))
            {
                builder.OldFace(faceId, oldfaceSegment.Type ?? 1);
            }
            else
            {
                throw new ArgumentException($"Invalid Face ID: {oldfaceSegment.Id}");
            }
        }
    }

    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not OldFaceEntity oldfaceEntity) throw new ArgumentException("Invalid entity type.");
        return Excluded.Contains(oldfaceEntity.FaceId) ? null : new OldFaceSegment(oldfaceEntity.FaceId);
    }
}