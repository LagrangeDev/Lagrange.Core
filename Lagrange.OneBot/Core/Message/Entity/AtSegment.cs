using System.Text.Json.Serialization;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class AtSegment
{
    public AtSegment(uint at) => At = at.ToString();

    [JsonPropertyName("qq")] public string At { get; set; }
}

public partial class AtSegment : IOneBotSegment<MentionEntity>
{
    string IOneBotSegment<MentionEntity>.Type => "at";

    public MentionEntity ToEntity() => new("", uint.Parse(At));

    public ISegment FromMessageEntity(MentionEntity entity) => new AtSegment(entity.Uin);
}