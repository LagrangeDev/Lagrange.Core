using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class FaceSegment
{
    public FaceSegment(int id) => Id = id.ToString();

    [JsonPropertyName("id")] public string Id { get; set; }
}

public partial class FaceSegment : ISegment
{
    string ISegment.Type => "face";

    public IMessageEntity ToEntity() => new FaceEntity(ushort.Parse(Id), false);

    public ISegment FromMessageEntity(IMessageEntity entity)
    {
        if (entity is not FaceEntity faceEntity) throw new ArgumentException("Invalid entity type.");

        return new FaceSegment(faceEntity.FaceId);
    }
}