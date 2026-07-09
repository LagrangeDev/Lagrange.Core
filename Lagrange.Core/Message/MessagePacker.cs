using System.Formats.Asn1;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.System;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Message.Entities;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;


namespace Lagrange.Core.Message;

internal class MessagePacker(BotContext context)
{
    private readonly List<IMessageEntity> _factory = MessageEntityRegistry.Create();

    public async Task<BotMessage> Parse(CommonMessage msg)
    {
        var contentHead = msg.ContentHead;
        var routingHead = msg.RoutingHead;

        var contact = await ResolveContact(contentHead.Type, routingHead);
        var receiver = await ResolveReceiver(contentHead.Type, routingHead);
        var message = new BotMessage(contact, receiver, DateTime.Now)
        {
            MessageId = contentHead.MsgUid, // MsgUid & 0xFFFFFFFF are the same to random
            Time = DateTimeOffset.FromUnixTimeSeconds(contentHead.Time).DateTime,
            Sequence = contentHead.Sequence,
            ClientSequence = contentHead.ClientSequence,
            Random = contentHead.Random
        };
        
        if (msg.MessageBody is null)
            return message;
        
        if (ParsePttRichText(msg.MessageBody.RichText) is { } record) 
        {
            message.Entities.Add(record);
        }
        else
        {
            var elems = msg.MessageBody.RichText.Elems;
            foreach (var elem in elems)
            {
                foreach (var factory in _factory)
                {
                    if (factory.Parse(elems, elem) is not { } entity) continue;

                    message.Entities.Add(entity);
                    break;
                }
            }
        }

        foreach (var entity in message.Entities)
        {
            await entity.Postprocess(context, message);
        }

        return message;
    }

    private async Task<BotContact> ResolveContact(int type, RoutingHead routingHead)
    {
        switch (type)
        {
            case 166:
                var friend = await context.CacheContext.ResolveFriend(routingHead.FromUin);
                return friend ?? new BotFriend(routingHead.FromUin, routingHead.FromUid, string.Empty, string.Empty, string.Empty, string.Empty, null!);

            case 141:
                return (await context.CacheContext.ResolveStranger(routingHead.ToUid)).CloneWithSource(routingHead.CommonC2C.FromTinyId);
            case 82:
                var items = await context.CacheContext.ResolveMember(routingHead.Group.GroupCode, routingHead.FromUin);
                if (items != null) return items.Value.Item2;

                var dummyGroup = new BotGroup(routingHead.Group.GroupCode, routingHead.Group.GroupName, 0, 0, 0, null, null, null);
                return new BotGroupMember(dummyGroup, routingHead.FromUin, routingHead.FromUid, routingHead.Group.GroupCard, GroupMemberPermission.Member, 0, routingHead.Group.GroupCard, null, DateTime.Now, DateTime.Now, DateTime.Now);

            default:
                throw new NotImplementedException();
        }
    }
    
    private async Task<BotContact> ResolveReceiver(int type, RoutingHead routingHead)
    {
        switch (type)
        {
            case 166:
                var friend = await context.CacheContext.ResolveFriend(routingHead.ToUin);
                if (friend == null)
                {
                    return new BotFriend(routingHead.ToUin, routingHead.ToUid, string.Empty, string.Empty, string.Empty, string.Empty, null!);
                }

                return friend;
            case 141:
                return (await context.CacheContext.ResolveStranger(routingHead.ToUid)).CloneWithSource(routingHead.CommonC2C.FromTinyId);
            case 82:
                var items = await context.CacheContext.ResolveMember(routingHead.Group.GroupCode, routingHead.ToUin);
                if (items == null)
                {
                    var dummyGroup = new BotGroup(routingHead.Group.GroupCode, routingHead.Group.GroupName, 0, 0, 0, null, null, null);
                    return new BotGroupMember(dummyGroup, routingHead.ToUin, routingHead.ToUid, routingHead.Group.GroupCard, GroupMemberPermission.Member, 0, routingHead.Group.GroupCard, null, DateTime.Now, DateTime.Now, DateTime.Now);
                }

                return items.Value.Item2;
            default:
                throw new NotImplementedException();
        }
    }

    public static ReadOnlyMemory<byte> Build(BotMessage message)
    {
        var routingHead = new SendRoutingHead();

        switch (message.Contact)
        {
            case BotFriend:
                routingHead.C2C = new C2C { PeerUin = message.Receiver.Uin, PeerUid = message.Receiver.Uid };
                break;
            case BotStranger:
                throw new NotSupportedException();
        }
        
        if (message.Receiver is BotGroup group)
        {
            routingHead.Group = new Grp { GroupUin = group.GroupUin };
        }

        var messageBody = new MessageBody();
        foreach (var entity in message.Entities)
        {
            if (entity.Build() is not { } elem) continue;
            messageBody.RichText.Elems.AddRange(elem);
        }

        var proto = new PbSendMsgReq
        {
            RoutingHead = routingHead,
            ContentHead = new SendContentHead
            {
                PkgNum = 1,
                PkgIndex = 0,
                DivSeq = 0,
                AutoReply = 0
            },
            MessageBody = messageBody,
            ClientSequence = message.ClientSequence,
            Random = message.Random,
        };
        return ProtoHelper.Serialize(proto);
    }

    public static ReadOnlyMemory<byte> BuildTrans0X211(BotFriend friend, FileUploadEventReq req, FileUploadEventResp resp, ulong clientSequence, uint random)
    {
        var extra = new FileExtra
        {
            File = new NotOnlineFile
            {
                FileType = 0,
                FileUuid = resp.FileId,
                FileMd5 = req.File10MMd5,
                FileName = req.FileName,
                FileSize = (ulong)req.FileStream.Length,
                SubCmd = 1,
                DangerLevel = 0,
                ExpireTime = (uint)DateTimeOffset.Now.AddDays(7).ToUnixTimeSeconds(),
                FileIdCrcMedia = resp.CrcMedia
            }
        };
            
        var proto = new PbSendMsgReq
        {
            RoutingHead = new SendRoutingHead
            {
                Trans0X211 = new Trans0X211
                {
                    ToUin = friend.Uin,
                    CcCmd = 4,
                    Uid = friend.Uid
                }
            },
            ContentHead = new SendContentHead
            {
                PkgNum = 1,
                PkgIndex = 0,
                DivSeq = 0,
                AutoReply = 0
            },
            MessageBody = new MessageBody
            {
                MsgContent = ProtoHelper.Serialize(extra)
            },
            ClientSequence = clientSequence,
            Random = random
        };
        return ProtoHelper.Serialize(proto);
    }

    public Task<CommonMessage> BuildFake(BotMessage msg)
    {
        var proto = new CommonMessage
        {
            RoutingHead = msg.Contact switch
            {
                BotGroupMember member => new RoutingHead
                {
                    Group = new CommonGroup
                    {
                        GroupCode = member.Group.GroupUin,
                        GroupCard = member.MemberCard ?? member.Uin.ToString(),
                        GroupCardType = 2
                    }
                },
                BotFriend friend => new RoutingHead { CommonC2C = new CommonC2C { Name = friend.Nickname } },
                BotStranger stranger => new RoutingHead { CommonC2C = new CommonC2C { Name = stranger.Nickname } },
                _ => throw new ArgumentOutOfRangeException(nameof(msg.Contact))
            },
            ContentHead = new ContentHead
            {
                Type = msg.Contact switch
                {
                    BotGroupMember => 82,
                    BotFriend => 166,
                    BotStranger => 141,
                    _ => throw new ArgumentOutOfRangeException(nameof(msg.Contact))
                },
                Random = msg.Random,
                Sequence = msg.Sequence,
                Time = new DateTimeOffset(msg.Time).ToUnixTimeSeconds(),
                ClientSequence = msg.ClientSequence,
                MsgUid = msg.MessageId,
            },
            MessageBody = new MessageBody { RichText = new RichText { Elems = [] } }
        };
        
        proto.RoutingHead.FromUin = msg.Contact.Uin;
        proto.RoutingHead.FromUid = context.CacheContext.ResolveCachedUid(msg.Contact.Uin) ?? "";
        if (msg.Receiver is BotFriend f)
        {
            proto.RoutingHead.ToUin = f.Uin;
            proto.RoutingHead.ToUid = context.CacheContext.ResolveCachedUid(f.Uin) ?? "";
        }

        foreach (var entity in msg.Entities)
        {
            if (entity.Build() is not { } elem) continue;
            proto.MessageBody.RichText.Elems.AddRange(elem);
        }

        return Task.FromResult(proto);
    }

    private RecordEntity? ParsePttRichText(RichText richText)
    {
        if (richText.Ptt is not { } ptt) return null;
        
        var kv = new BinaryPacket(stackalloc byte[100]);
        kv.Write("filetype", Prefix.Int32 | Prefix.LengthOnly);
        kv.Write("0", Prefix.Int32 | Prefix.LengthOnly);
        kv.Write("codec", Prefix.Int32 | Prefix.LengthOnly);
        kv.Write("1", Prefix.Int32 | Prefix.LengthOnly);
            
        Span<byte> innerSpan = stackalloc byte[200];
        Span<byte> outerSpan = stackalloc byte[300];
            
        var inner = new AsnWriter(AsnEncodingRules.DER);
        inner.PushSequence();
        inner.WriteInteger(1);
        inner.WriteInteger(0);
        inner.WriteInteger((int)(context.BotUin < 0x8000_0000 ? context.BotUin : context.BotUin - 0x1_0000_0000));
        inner.WriteOctetString(ptt.GroupFileKey ?? ptt.FileUuid ?? ReadOnlySpan<byte>.Empty);
        inner.WriteInteger(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        inner.WriteOctetString(kv.CreateReadOnlySpan());
        inner.PopSequence();
        inner.TryEncode(innerSpan, out int length);

        var outer = new AsnWriter(AsnEncodingRules.DER);
        outer.PushSequence();
        outer.WriteInteger(1); // version
        outer.WriteOctetString(innerSpan[..length]); // inner
        outer.WriteOctetString(ReadOnlySpan<byte>.Empty);   // “empty”
        outer.PopSequence();
            
        outer.TryEncode(outerSpan, out length);
            
        return new RecordEntity
        {
            FileUrl = $"https://grouptalk.c2c.qq.com/?ver=2&rkey={Convert.ToHexString(outerSpan[..length])}&voice_codec=1&filetype=0",
            FileName = ptt.FileName,
            FileSize = ptt.FileSize,
            FileMd5 = Convert.ToHexString(ptt.FileMd5 ?? [])
        };
    }
}
