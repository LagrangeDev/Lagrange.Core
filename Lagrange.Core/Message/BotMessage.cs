using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Message;

public partial class BotMessage
{
    internal BotMessage(BotContact contact, BotContact receiver, long time)
    {
        Contact = contact;
        Receiver = receiver;
        Time = time;
    }

    internal BotMessage(MessageChain chain, BotContact contact, BotContact receiver, long time)
    {
        Entities = chain;
        Contact = contact;
        Receiver = receiver;
        Time = time;
    }

    public BotContact Contact { get; }

    public BotContact Receiver { get; }

    public MessageType Type => Contact switch
    {
        BotGroupMember _ => MessageType.Group,
        BotFriend _ => MessageType.Private,
        BotStranger _ => MessageType.Temp,
        _ => throw new ArgumentOutOfRangeException(nameof(Contact))
    };

    public long Time { get; set; }

    public MessageChain Entities { get; } = [];

    internal ulong MessageId { get; set; }

    internal uint Random { get; init; }

    public ulong Sequence { get; set; }

    public ulong ClientSequence { get; init; } = (ulong)new Random().NextInt64(10000, 99999);
}