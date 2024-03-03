using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Database;

namespace Lagrange.OneBot.Message.Entity;

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
            reply.TargetChain ??= (MessageChain)Database.GetCollection<MessageRecord>().FindOne(x => x.MessageHash == int.Parse(reply.MessageId));

            var build = MessagePacker.Build(reply.TargetChain, "");
            var virtualElem = build.Body?.RichText?.Elems;
            if (virtualElem != null) reply.TargetChain.Elements.AddRange(virtualElem);
            
            builder.Forward(reply.TargetChain);
        }
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not ForwardEntity forward || Database is null) throw new ArgumentException("The entity is not a forward entity.");
        
        var collection = Database.GetCollection<MessageRecord>();
            
        uint friendUin = chain.FriendUin;  // such action is to break the BsonExpression from MessageChain into uint
        uint groupUin = chain.GroupUin ?? 0;
            
        var query = chain.IsGroup
            ? collection.Query().Where(x => x.Sequence == forward.Sequence).Where(x => x.GroupUin == groupUin)
            : collection.Query().Where(x => x.Sequence == forward.Sequence).Where(x => x.FriendUin == friendUin);
        var target = query.FirstOrDefault();

        return target == null
            ? new ReplySegment { MessageId = 0.ToString() }
            : new ReplySegment { MessageId = target.MessageHash.ToString() };
    }
}