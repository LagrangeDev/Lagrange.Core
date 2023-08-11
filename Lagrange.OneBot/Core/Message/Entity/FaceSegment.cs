using System.Text.Json.Serialization;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class FaceSegment
{
    public FaceSegment(int id) => Id = id.ToString();

    [JsonPropertyName("id")] public string Id { get; set; }
}

public partial class FaceSegment : IOneBotSegment<FaceEntity>
{
    string IOneBotSegment<FaceEntity>.Type => "face";

    public FaceEntity ToEntity() => new(ushort.Parse(Id), false);

    public ISegment FromMessageEntity(FaceEntity entity) => new FaceSegment(entity.FaceId);
}