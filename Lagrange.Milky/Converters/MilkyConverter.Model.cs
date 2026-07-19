using System;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Milky.Events.Converters;
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

    public GroupNotificationBase ToGroupNotification(BotGroupNotificationBase notification) => notification switch
    {
        BotGroupJoinNotification join => new JoinRequestGroupNotification
        {
            GroupId = join.GroupUin,
            NotificationSeq = (long)join.Sequence,
            IsFiltered = join.IsFiltered,
            InitiatorId = join.TargetUin,
            State = join.State switch
            {
                BotGroupNotificationState.Wait => "pending",
                BotGroupNotificationState.Accept => "accepted",
                BotGroupNotificationState.Reject => "rejected",
                BotGroupNotificationState.Ignore => "ignored",
                _ => throw new NotSupportedException(),
            },
            OperatorId = join.OperatorUin,
            Comment = join.Comment,
        },
        BotGroupSetAdminNotification groupSet => new AdminChangeGroupNotification
        {
            GroupId = groupSet.GroupUin,
            NotificationSeq = (long)groupSet.Sequence,
            TargetUserId = groupSet.TargetUin,
            IsSet = true,
            OperatorId = groupSet.OperatorUin,
        },
        BotGroupUnsetAdminNotification groupUnset => new AdminChangeGroupNotification
        {
            GroupId = groupUnset.GroupUin,
            NotificationSeq = (long)groupUnset.Sequence,
            TargetUserId = groupUnset.TargetUin,
            IsSet = false,
            OperatorId = groupUnset.OperatorUin,
        },
        BotGroupKickNotification kick => new KickGroupNotification
        {
            GroupId = kick.GroupUin,
            NotificationSeq = (long)kick.Sequence,
            TargetUserId = kick.TargetUin,
            OperatorId = kick.OperatorUin,
        },
        BotGroupExitNotification exit => new QuitGroupNotification
        {
            GroupId = exit.GroupUin,
            NotificationSeq = (long)exit.Sequence,
            TargetUserId = exit.TargetUin,
        },
        BotGroupInviteNotification invite => new InvitedJoinRequestGroupNotification
        {
            GroupId = invite.GroupUin,
            NotificationSeq = (long)invite.Sequence,
            InitiatorId = invite.InviterUin,
            TargetUserId = invite.TargetUin,
            State = invite.State switch
            {
                BotGroupNotificationState.Wait => "pending",
                BotGroupNotificationState.Accept => "accepted",
                BotGroupNotificationState.Reject => "rejected",
                BotGroupNotificationState.Ignore => "ignored",
                _ => throw new NotSupportedException(),
            },
            OperatorId = invite.OperatorUin,
        },
        _ => throw new NotSupportedException(),
    };

    public GroupFile ToGroupFile(BotFileEntry entry, long groupId) => new()
    {
        GroupId = groupId,
        FileId = entry.FileId,
        FileName = entry.FileName,
        ParentFolderId = entry.ParentDirectory,
        FileSize = (long)entry.FileSize,
        UploadedTime = entry.UploadedTime.ToUnixTimeSeconds(),
        ExpireTime = entry.ExpireTime.ToUnixTimeSeconds(),
        UploaderId = entry.UploaderUin,
        DownloadedTimes = (int)entry.DownloadedTimes,
    };

    public GroupFolder ToGroupFolder(BotFolderEntry entry, long groupId) => new()
    {
        GroupId = groupId,
        FolderId = entry.FolderId,
        ParentFolderId = entry.ParentFolderId,
        FolderName = entry.FolderName,
        CreatedTime = entry.CreateTime.ToUnixTimeSeconds(),
        LastModifiedTime = entry.ModifiedTime.ToUnixTimeSeconds(),
        CreatorId = entry.CreatorUin,
        FileCount = (int)entry.TotalFileCount,
    };
}
