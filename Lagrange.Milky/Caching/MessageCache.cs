using System;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Lagrange.Milky.Caching;

public class MessageCache(BotContext lagrange, ILoggerFactory loggerFactory)
{
    private readonly BotContext _lagrange = lagrange;

    private readonly MemoryCache _cache = new(new MemoryCacheOptions { }, loggerFactory);

    public BotMessage? Set(BotMessage message) => _cache.Set(
        new MessageKey
        {
            Type = message.Type,
            PeerUin = message.Contact switch
            {
                BotFriend sender => sender.Uin == _lagrange.BotUin ? message.Receiver.Uin : sender.Uin,
                BotGroupMember sender => sender.Group.Uin,
                BotStranger sender => sender.Uin == _lagrange.BotUin ? message.Receiver.Uin : sender.Uin,
                _ => message.Contact.Uin,
            },
            Sequence = message.Type switch
            {
                MessageType.Private => message.ClientSequence,
                _ => message.Sequence,
            },
        },
        message,
        TimeSpan.FromSeconds(1)
    );
    public BotMessage? Get(MessageType type, long uin, ulong sequence) => _cache.Get<BotMessage>(new MessageKey
    {
        Type = type,
        PeerUin = uin,
        Sequence = sequence,
    });

    private readonly struct MessageKey
    {
        public required MessageType Type { get; init; }
        public required long PeerUin { get; init; }
        public required ulong Sequence { get; init; }

        public override readonly bool Equals(object? obj) => obj is MessageKey other
            && Type == other.Type
            && PeerUin == other.PeerUin
            && Sequence == other.Sequence;

        public override readonly int GetHashCode() => HashCode.Combine(Type, PeerUin, Sequence);
    }
}