using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Message.Entities;

public class ReplyEntity : IMessageEntity
{
    public ulong SrcUid { get; private init; }

    public ulong SrcSequence { get; private init; }

    public BotContact? Source { get; private set; }

    internal List<Elem> Elems { get; private init; } = [];

    public long SourceUin { get; private set; } // only for storage, not used in protocol

    public long SourceTime { get; private init; }

    public MessageChain SourceEntities { get; } = [];

    public ReplyEntity(BotMessage source)
    {
        Source = source.Contact;
        SrcUid = source.MessageId;
        SrcSequence = source.Sequence;
    }

    async Task IMessageEntity.Postprocess(BotContext context, BotMessage message)
    {
        // the source of a quote in a forwarded message is usually not a contact of the bot
        if (message.IsForwarded) return;

        Source = message.Contact switch
        {
            BotFriend => await context.CacheContext.ResolveFriend(SourceUin),
            BotGroupMember s => (await context.CacheContext.ResolveMember(s.Group.GroupUin, SourceUin)).GetValueOrDefault().Item2,
            BotStranger => await context.CacheContext.ResolveStranger(SourceUin),
            _ => null
        };
    }

    public ReplyEntity() { }

    string IMessageEntity.ToPreviewString() => string.Empty;

    Elem[] IMessageEntity.Build()
    {
        if (Source == null) return [];

        var srcMsg = new Elem
        {
            SrcMsg = new SourceMsg
            {
                OrigSeqs = [SrcSequence],
                SenderUin = 0,
                Time = (uint)DateTimeOffset.Now.ToUnixTimeSeconds(),
                Flag = 0, // intentional, force the client to fetch the original message
                Elems = Elems.Select(ProtoHelper.Serialize).ToList(),
                PbReserve = ProtoHelper.Serialize(new SourceMsgResvAttr
                {
                    OriMsgType = 2,
                    SourceMsgId = SrcUid,
                    SenderUid = Source.Uid
                }),
            }
        };

        return Source is not BotGroupMember ? [srcMsg] : [srcMsg, new Elem
        {
            Text = new Text
            {
                TextMsg = $"@{Source.Nickname}",
                PbReserve = ProtoHelper.Serialize(new TextResvAttr
                {
                    AtType = 2u, AtMemberUin = 0, AtMemberTinyid = 0, AtMemberUid = Source.Uid
                })
            }
        }];
    }

    IMessageEntity? IMessageEntity.Parse(List<Elem> elements, Elem target)
    {
        if (target.SrcMsg is { } srcMsg)
        {
            var resvAttr = ProtoHelper.Deserialize<SourceMsgResvAttr>(srcMsg.PbReserve.Span);

            return new ReplyEntity
            {
                SrcUid = resvAttr.SourceMsgId,
                SrcSequence = srcMsg.OrigSeqs[0],
                Elems = (srcMsg.Elems ?? []).Select(x => ProtoHelper.Deserialize<Elem>(x.Span)).ToList(),
                SourceUin = (long)srcMsg.SenderUin,
                SourceTime = srcMsg.Time,
            };
        }

        return null;
    }
}