using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class LongMsgSegment(string name)
{
    public LongMsgSegment() : this("") { }

    [JsonPropertyName("id")] [CQProperty] public string Name { get; set; } = name;
}

[SegmentSubscriber(typeof(LongMsgSegment), "longmsg")]
public partial class LongMsgSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is LongMsgSegment longMsg) builder.Add(new LongMsgEntity(longMsg.Name));
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not LongMsgEntity longMsg) throw new ArgumentException("Invalid entity type.");

        return new ForwardSegment(longMsg.ResId ?? "");
    }
}