using System.Text.Json.Serialization;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class TextSegment
{
    public TextSegment(string text) => Text = text;

    [JsonPropertyName("text")] public string Text { get; set; }
}

public partial class TextSegment : IOneBotSegment<TextEntity>
{
    string IOneBotSegment<TextEntity>.Type => "text";

    public TextEntity ToEntity() => new(Text);

    public ISegment FromMessageEntity(TextEntity entity) => new TextSegment(entity.Text);
}