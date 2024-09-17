using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Database;

[Serializable]
public class MessageRecord
{
    public uint FriendUin { get; set; }

    public uint GroupUin { get; set; }

    public uint Sequence { get; set; }

    public uint ClientSequence { get; set; }

    public DateTime Time { get; set; }

    public ulong MessageId { get; set; }

    public BotFriend? FriendInfo { get; set; }

    public BotGroupMember? GroupMemberInfo { get; set; }

    public List<IMessageEntity> Entities { get; set; } = [];

    public int MessageHash { get; set; }

    public uint TargetUin { get; set; }

    static MessageRecord()
    {
        Vector2Mapper.RegisterType(); // I HATE THIS
    }

    public static explicit operator MessageChain(MessageRecord record)
    {
        var chain = record.GroupUin != 0
            ? new MessageChain(record.GroupUin, record.FriendUin, record.Sequence, record.MessageId)
            : new MessageChain(record.FriendUin, string.Empty, string.Empty, record.TargetUin, record.Sequence, record.ClientSequence, record.MessageId);

        chain.Time = record.Time;
        chain.FriendInfo = record.FriendInfo;
        chain.GroupMemberInfo = record.GroupMemberInfo;

        chain.AddRange(record.Entities);
        return chain;
    }

    public static explicit operator MessageRecord(MessageChain chain) => new()
    {
        FriendUin = chain.FriendUin,
        GroupUin = chain.GroupUin ?? 0,
        Sequence = chain.Sequence,
        ClientSequence = chain.ClientSequence,
        Time = chain.Time,
        MessageId = chain.MessageId,
        FriendInfo = chain.FriendInfo,
        GroupMemberInfo = chain.GroupMemberInfo,
        Entities = chain,
        MessageHash = CalcMessageHash(chain.MessageId, chain.Sequence),
        TargetUin = chain.TargetUin
    };

    public static int CalcMessageHash(ulong msgId, uint seq)
    {
        var messageId = BitConverter.GetBytes(msgId);
        var sequence = BitConverter.GetBytes(seq);

        byte[] id = [messageId[0], messageId[1], sequence[0], sequence[1]];
        return BitConverter.ToInt32(id.AsSpan());
    }

    public static int CalcMessageHash(uint random, uint seq)
    {
        var messageId = BitConverter.GetBytes(random);
        var sequence = BitConverter.GetBytes(seq);

        byte[] id = [messageId[0], messageId[1], sequence[0], sequence[1]];
        return BitConverter.ToInt32(id.AsSpan());
    }
}
