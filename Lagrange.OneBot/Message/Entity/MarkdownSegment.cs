using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class MarkdownSegment(string content)
{
    public MarkdownSegment(): this("") { }
    
    [JsonPropertyName("content")] [CQProperty] public string Content { get; set; } = content;
}

[SegmentSubscriber(typeof(MarkdownEntity), "markdown")]
public partial class MarkdownSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is MarkdownSegment markdown) builder.Markdown(markdown.Content);
    }

    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        return null;
    }
}