using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Message;

public partial class BotMessage
{
    public static BotMessage CreateCustomGroup(long groupUin, long memberUin, string memberCard, long time, MessageChain chain)
    {
        long now = DateTimeOffset.Now.ToUnixTimeSeconds();

        var dummyGroup = new BotGroup(groupUin, string.Empty, 0, 0, 0, null, null, null);
        var dummyMember = new BotGroupMember(dummyGroup, memberUin, string.Empty, memberCard, GroupMemberPermission.Member, 0, memberCard, null, now, now, now);
        return new BotMessage(chain, dummyMember, dummyGroup, time);
    }

    public static BotMessage CreateCustomFriend(long senderUin, string senderName, long receiverUin, string receiverName, long time, MessageChain chain)
    {
        var dummySender = new BotFriend(senderUin, senderName, string.Empty, string.Empty, string.Empty, string.Empty, null!);
        var dummyReceiver = new BotFriend(receiverUin, receiverName, string.Empty, string.Empty, string.Empty, string.Empty, null!);
        return new BotMessage(chain, dummySender, dummyReceiver, time);
    }
}