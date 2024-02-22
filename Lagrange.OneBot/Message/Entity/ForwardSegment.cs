using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class ForwardSegment(string name)
{
    public ForwardSegment() : this("") { }

    [JsonPropertyName("id")] [CQProperty] public string Name { get; set; } = name;
}

[SegmentSubscriber(typeof(MultiMsgEntity), "forward")]
public partial class ForwardSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is ForwardSegment forward) builder.Add(new MultiMsgEntity(forward.Name));
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not MultiMsgEntity multiMsg) throw new ArgumentException("Invalid entity type.");

        return new ForwardSegment(multiMsg.ResId ?? "");
    }
}