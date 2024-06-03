using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class KeyboardSegment(KeyboardData content)
{
    public KeyboardSegment(): this(new KeyboardData()) { }
    
    [JsonPropertyName("content")] public KeyboardData Content { get; set; } = content;
}

[SegmentSubscriber(typeof(KeyboardEntity), "keyboard")]
public partial class KeyboardSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is KeyboardSegment keyboard) builder.Keyboard(keyboard.Content);
    }

    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        return null;
    }
}