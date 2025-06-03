using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Database;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class ReplySegment(uint messageId)
{
    public ReplySegment() : this(0) { }

    [JsonPropertyName("id")][CQProperty] public string MessageId { get; set; } = messageId.ToString();
}

[SegmentSubscriber(typeof(ForwardEntity), "reply")]
public partial class ReplySegment : SegmentBase
{
    internal MessageChain? TargetChain { get; set; }

    internal uint Sequence { get; private set; }

    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is ReplySegment reply && Realm is not null)
        {
            var chain = Realm.Do<MessageChain>(realm => realm.All<MessageRecord>()
                .First(record => record.Id == int.Parse(reply.MessageId)));

            reply.TargetChain ??= chain;

            var build = MessagePacker.Build(reply.TargetChain, "");
            var virtualElem = build.Body?.RichText?.Elems;
            if (virtualElem != null) reply.TargetChain.Elements.AddRange(virtualElem);

            builder.Forward(reply.TargetChain);
        }
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not ForwardEntity forward || Realm is null) throw new ArgumentException("The entity is not a forward entity.");

        int? id;
        if (chain.IsGroup)
        {
            id = MessageRecord.CalcMessageHash(forward.MessageId, forward.Sequence);
        }
        else
        {
            id = Realm.Do(realm => realm.All<MessageRecord>()
                .FirstOrDefault(record => record.FromUinLong == chain.FriendUin
                    && record.ClientSequenceLong == forward.ClientSequence)?
                .Id);
        }

        return new ReplySegment { MessageId = (id ?? 0).ToString() };
    }
}