using System;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;
using Lagrange.Milky.Extensions;
using Lagrange.Milky.Models.Messages;

namespace Lagrange.Milky.Converters;

public partial class MilkyConverter
{
    public async Task<IncomingMessageBase> ToIncomingMessageAsync(BotMessage message, CancellationToken ct = default) => message.Type switch
    {
        MessageType.Private => await ToFriendMessageAsync(message, ct),
        MessageType.Group => await ToGroupMessageAsync(message, ct),
        _ => throw new NotSupportedException(),
    };

    public async Task<FriendIncomingMessage> ToFriendMessageAsync(BotMessage message, CancellationToken ct = default)
    {
        var sender = (BotFriend)message.Contact;
        long peerUin = sender.Uin == _lagrange.BotUin ? message.Receiver.Uin : sender.Uin;
        return new FriendIncomingMessage()
        {
            PeerId = peerUin,
            MessageSeq = (long)message.ClientSequence,
            SenderId = message.Contact.Uin,
            Time = message.Time.ToUnixTimeSeconds(),
            Segments = await ToIncomingSegmentsAsync(message.Entities, message.Type, peerUin, ct),
            Friend = ToFriend(sender),
        };
    }

    private async Task<GroupIncomingMessage> ToGroupMessageAsync(BotMessage message, CancellationToken ct = default)
    {
        var member = (BotGroupMember)message.Contact;
        var group = member.Group;
        return new()
        {
            PeerId = group.Uin,
            MessageSeq = (long)message.Sequence,
            SenderId = message.Contact.Uin,
            Time = message.Time.ToUnixTimeSeconds(),
            Segments = await ToIncomingSegmentsAsync(message.Entities, message.Type, group.Uin, ct),
            Group = ToGroup(group),
            GroupMember = ToGroupMember(member),
        };
    }
}