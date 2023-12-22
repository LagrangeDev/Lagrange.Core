using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;

namespace Lagrange.OneBot.Database;

[Serializable]
public class MessageRecord
{
    public uint FriendUin { get; set; }
    
    public uint GroupUin { get; set; }

    public uint Sequence { get; set; }
    
    public DateTime Time { get; set; }
    
    public ulong MessageId { get; set; }
    
    public BotFriend? FriendInfo { get; internal set; }
    
    public BotGroupMember? GroupMemberInfo { get; internal set; }

    public List<IMessageEntity> Entities { get; set; } = [];
    
    public uint MessageHash { get; set; }

    public static explicit operator MessageChain(MessageRecord record)
    {
        var chain = record.GroupUin != 0 
            ? new MessageChain(record.GroupUin, record.FriendUin, record.Sequence, record.MessageId) 
            : new MessageChain(record.FriendUin, string.Empty, string.Empty, record.MessageId);

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
        Time = chain.Time,
        MessageId = chain.MessageId,
        FriendInfo = chain.FriendInfo,
        GroupMemberInfo = chain.GroupMemberInfo,
        Entities = chain,
        MessageHash = CalcMessageHash(chain.MessageId, chain.Sequence)
    };

    private static uint CalcMessageHash(ulong msgId, uint seq)
    {
        var messageId = BitConverter.GetBytes(msgId);
        var sequence = BitConverter.GetBytes(seq);

        byte[] id = [messageId[6], messageId[7], sequence[0], sequence[1]];
        return BitConverter.ToUInt32(id.AsSpan());
    }
}