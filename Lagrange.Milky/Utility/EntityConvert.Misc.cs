using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Milky.Entity;
using Lagrange.Milky.Extension;

namespace Lagrange.Milky.Utility;

public partial class EntityConvert
{
    public Friend Friend(BotFriend friend) => new(
        friend.Uin,
        friend.Qid,
        friend.Nickname,
        friend.Gender switch
        {
            BotGender.Male => "male",
            BotGender.Female => "female",
            BotGender.Unset or
            BotGender.Unknown => "unknown",
            _ => throw new NotSupportedException(),
        },
        friend.Remarks,
        FriendCategory(friend.Category)
    );

    private FriendCategory FriendCategory(BotFriendCategory category) => new(category.Id, category.Name);

    public Group Group(BotGroup group) => new(
        group.GroupUin,
        group.GroupName,
        group.MemberCount,
        group.MaxMember,
        group.GroupRemark ?? string.Empty,
        group.CreateTime,
        group.Description ?? string.Empty,
        group.Question ?? string.Empty,
        group.Announcement ?? string.Empty
    );

    public GroupMember GroupMember(BotGroupMember member) => new(
        member.Uin,
        member.Nickname,
        member.Gender switch
        {
            BotGender.Male => "male",
            BotGender.Female => "female",
            BotGender.Unset or
            BotGender.Unknown => "unknown",
            _ => throw new NotSupportedException(),
        },
        member.Group.Uin,
        member.MemberCard ?? string.Empty,
        member.SpecialTitle ?? string.Empty,
        member.GroupLevel,
        member.Permission switch
        {
            GroupMemberPermission.Member => "member",
            GroupMemberPermission.Owner => "owner",
            GroupMemberPermission.Admin => "admin",
            _ => throw new NotSupportedException(),
        },
        member.JoinTime.ToUnixTimeSeconds(),
        member.LastMsgTime.ToUnixTimeSeconds(),
        member.ShutUpTimestamp.ToUnixTimeSeconds()
    );
}
