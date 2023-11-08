using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Core.Message.Entity;

public partial class ForwardSegment(string name)
{
    public ForwardSegment() : this("") { }

    [JsonPropertyName("id")] public string Name { get; set; } = name;
}

[SegmentSubscriber(typeof(ForwardSegment), "forward")]
public partial class ForwardSegment : ISegment
{
    public IMessageEntity ToEntity() => throw new InvalidOperationException("Only Receive but not send");
    
    public void Build(MessageBuilder builder, ISegment segment) => throw new InvalidOperationException();

    public ISegment FromEntity(IMessageEntity entity)
    {
        if (entity is not MultiMsgEntity multiMsg) throw new ArgumentException("Invalid entity type.");

        return new ForwardSegment(multiMsg.ResId ?? "");
    }
}