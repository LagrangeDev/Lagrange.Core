using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class GreyTipSegment(string content)
{
    public GreyTipSegment(): this("") {}
    
    [JsonPropertyName("content")] [CQProperty] public string Content { get; set; } = content;
}

[SegmentSubscriber(typeof(GreyTipEntity), "greytip")]
public partial class GreyTipSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is GreyTipSegment greyTip) builder.GeryTip(greyTip.Content);
    }

    public override SegmentBase? FromEntity(MessageChain chain, IMessageEntity entity)
    {
        return null;
    }
}