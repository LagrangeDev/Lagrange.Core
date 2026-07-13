using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entities;
using Lagrange.Milky.Extensions;
using Lagrange.Milky.Models.Segments;

namespace Lagrange.Milky.Converters;

public partial class MilkyConverter
{
    public async Task<IReadOnlyList<IncomingSegmentBase>> ToIncomingSegmentsAsync(MessageChain chain, MessageType type, long ownerPeerUin, CancellationToken ct = default)
    {
        List<IncomingSegmentBase> result = new(chain.Count);
        foreach (var entity in chain)
        {
            var segment = await ToIncomingSegmentAsync(entity, type, ownerPeerUin, ct);
            if (segment != null) result.Add(segment);
        }
        return result;
    }

    private async Task<IncomingSegmentBase?> ToIncomingSegmentAsync(IMessageEntity entity, MessageType type, long ownerPeerUin, CancellationToken ct = default) => entity switch
    {
        TextEntity text => new TextIncomingSegment { Data = new TextIncomingSegmentData { Text = text.Text } },
        MentionEntity mention => mention.Uin == 0
            ? new MentionAllIncomingSegment { Data = new object(), }
            : new MentionIncomingSegment
            {
                Data = new MentionIncomingSegmentData
                {
                    UserId = mention.Uin,
                    Name = mention.Display ?? string.Empty,
                }
            },
        // TODO: face is not implemented in core
        ReplyEntity reply => await ToReplyIncomingSegmentAsync(reply, type, ownerPeerUin, ct),
        ImageEntity image => new ImageIncomingSegment
        {
            Data = new ImageIncomingSegmentData
            {
                ResourceId = image.FileUuid,
                TempUrl = image.FileUrl,
                Width = (int)image.ImageSize.X,
                Height = (int)image.ImageSize.Y,
                Summary = image.Summary,
                SubType = image.SubType switch
                {
                    0 => "normal",
                    _ => "sticker",
                }
            }
        },
        RecordEntity record => new RecordIncomingSegment
        {
            Data = new RecordIncomingSegmentData
            {
                ResourceId = record.FileUuid,
                TempUrl = record.FileUrl,
                Duration = (int)record.RecordLength,
            }
        },
        VideoEntity video => new VideoIncomingSegment
        {
            Data = new VideoIncomingSegmentData
            {
                ResourceId = video.FileUuid,
                TempUrl = video.FileUrl,
                Width = (int)video.VideoSize.X,
                Height = (int)video.VideoSize.Y,
                Duration = (int)video.VideoLength,
            }
        },
        // TODO: file is not implemented in core
        MultiMsgEntity forward => new ForwardIncomingSegment
        {
            Data = new ForwardIncomingSegmentData
            {
                ForwardId = forward.ResId ?? throw new Exception(""),
                Title = string.Empty,   // TODO: field is not implemented in core
                Preview = [],           // TODO: field is not implemented in core
                Summary = string.Empty, // TODO: field is not implemented in core
            }
        },
        // TODO: market_face is not implemented in core
        LightAppEntity lightApp => new LightAppIncomingSegment
        {
            Data = new LightAppIncomingSegmentData
            {
                AppName = lightApp.AppName,
                JsonPayload = lightApp.Payload,
            }
        },
        // TODO: xml is not implemented in core
        _ => null,
    };

    private async Task<ReplyIncomingSegment> ToReplyIncomingSegmentAsync(ReplyEntity reply, MessageType type, long ownerPeerUin, CancellationToken ct = default)
    {
        var message = _cache.Get(type, ownerPeerUin, reply.SrcSequence)
            ?? (type switch
            {
                MessageType.Private => await _lagrange.GetC2CMessage(
                    ownerPeerUin,
                    reply.SrcSequence,
                    reply.SrcSequence
                ).WaitAsync(ct),
                MessageType.Group => await _lagrange.GetGroupMessage(
                    ownerPeerUin,
                    reply.SrcSequence,
                    reply.SrcSequence
                ).WaitAsync(ct),
                _ => throw new NotSupportedException(),
            }).First();

        return new ReplyIncomingSegment
        {
            Data = new ReplyIncomingSegmentData
            {
                MessageSeq = message.Type switch
                {
                    MessageType.Private => (long)message.ClientSequence,
                    _ => (long)message.Sequence,
                },
                SenderId = message.Contact.Uin,
                SenderName = message.Contact switch
                {
                    BotFriend sender => sender.Nickname,
                    BotGroupMember member => member.Nickname,
                    BotStranger stranger => stranger.Nickname,
                    _ => throw new NotSupportedException(),
                },
                Time = message.Time.ToUnixTimeSeconds(),
                Segments = await ToIncomingSegmentsAsync(message.Entities, type, ownerPeerUin, ct),
            }
        };
    }
}