using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class TextSegment
{
    public TextSegment(string text) => Text = text;

    [JsonPropertyName("text")] public string Text { get; set; }
}

public partial class TextSegment : ISegment
{
    string ISegment.Type => "text";

    public IMessageEntity ToEntity() => new TextEntity(Text);

    public ISegment FromMessageEntity(IMessageEntity entity)
    {
        if (entity is not TextEntity textEntity) throw new ArgumentException("Invalid entity type.");
        
        return new TextSegment(textEntity.Text);
    }
}