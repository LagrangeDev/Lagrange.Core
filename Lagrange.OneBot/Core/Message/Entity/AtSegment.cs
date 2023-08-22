using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class AtSegment
{
    public AtSegment(uint at) => At = at.ToString();

    [JsonPropertyName("qq")] public string At { get; set; }
}

public partial class AtSegment : ISegment
{
    string ISegment.Type => "at";

    public IMessageEntity ToEntity() => new MentionEntity("", uint.Parse(At));

    public ISegment FromMessageEntity(IMessageEntity entity)
    {
        if (entity is not MentionEntity mentionEntity) throw new ArgumentException("Invalid entity type.");
        
        return new AtSegment(mentionEntity.Uin);
    }
}