using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Database;
using LiteDB;

namespace Lagrange.OneBot.Core.Message.Entity;

[Serializable]
public partial class ReplySegment(uint messageId)
{
    public ReplySegment() : this(0) { }

    [JsonPropertyName("id")] [CQProperty] public string MessageId { get; set; } = messageId.ToString();
}

[SegmentSubscriber(typeof(ForwardEntity), "reply")]
public partial class ReplySegment : SegmentBase
{
    internal MessageChain? TargetChain { get; set; }
    
    internal uint Sequence { get; private set; }
    
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is ReplySegment reply && Database is not null)
        {
            reply.TargetChain ??= (MessageChain)Database.GetCollection<MessageRecord>().FindOne(x => x.MessageHash == uint.Parse(reply.MessageId));
            builder.Forward(reply.TargetChain);
        }
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not ForwardEntity forward || Database is null) throw new ArgumentException("The entity is not a forward entity.");

        var collection = Database.GetCollection<MessageRecord>();
        var target = chain.IsGroup 
            ? collection.FindOne(x => x.Sequence == forward.Sequence && x.GroupUin == chain.GroupUin) 
            : collection.FindOne(x => x.Sequence == forward.Sequence && x.FriendUin == chain.FriendUin);

        return new ReplySegment
        {
            MessageId = target.MessageHash.ToString()
        };
    }
}