using System;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Milky.Extensions;
using Lagrange.Milky.Models;

namespace Lagrange.Milky.Converters;

public partial class MilkyConverter
{
    public Friend ToFriend(BotFriend friend) => new()
    {
        UserId = friend.Uin,
        Nickname = friend.Nickname,
        Sex = friend.Gender switch
        {
            BotGender.Male => "male",
            BotGender.Female => "female",
            _ => "unknown"
        },
        Qid = friend.Qid,
        Remark = friend.Remarks,
        Category = ToFriendCategory(friend.Category),
    };

    public Group ToGroup(BotGroup group) => new()
    {
        GroupId = group.Uin,
        GroupName = group.GroupName,
        MemberCount = group.MemberCount,
        MaxMemberCount = group.MaxMember,
        Remark = group.GroupRemark ?? string.Empty,
        CreatedTime = group.CreateTime,
        Description = group.Description ?? string.Empty,
        Question = group.Question ?? string.Empty,
        Announcement = group.Announcement ?? string.Empty,
    };

    public GroupMember ToGroupMember(BotGroupMember member) => new()
    {
        UserId = member.Uin,
        Nickname = member.Nickname,
        Sex = member.Gender switch
        {
            BotGender.Male => "male",
            BotGender.Female => "female",
            _ => "unknown"
        },
        GroupId = member.Group.Uin,
        Card = member.MemberCard ?? string.Empty,
        Title = member.SpecialTitle ?? string.Empty,
        Level = member.GroupLevel,
        Role = member.Permission switch
        {
            GroupMemberPermission.Owner => "owner",
            GroupMemberPermission.Admin => "admin",
            GroupMemberPermission.Member => "member",
            _ => throw new NotSupportedException(),
        },
        JoinTime = member.JoinTime.ToUnixTimeSeconds(),
        LastSentTime = member.LastMsgTime.ToUnixTimeSeconds(),
        ShutUpEndTime = member.ShutUpTimestamp.ToUnixTimeSeconds()
    };

    private FriendCategory ToFriendCategory(BotFriendCategory category) => new()
    {
        CategoryId = category.Id,
        CategoryName = category.Name,
    };
}
