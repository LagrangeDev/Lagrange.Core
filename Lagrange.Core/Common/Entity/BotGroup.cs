namespace Lagrange.Core.Common.Entity;

public class BotGroup(
    long groupUin,
    string groupName,
    int memberCount,
    int maxMember,
    long createTime,
    string? description,
    string? question,
    string? announcement,
    string? groupRemark = null) : BotContact
{
    public long GroupUin { get; } = groupUin;

    public string GroupName { get; } = groupName ?? string.Empty;

    public string? GroupRemark { get; } = groupRemark;

    public int MemberCount { get; } = memberCount;

    public int MaxMember { get; } = maxMember;

    public long CreateTime { get; } = createTime;

    public string? Description { get; } = description;

    public string? Question { get; } = question;

    public string? Announcement { get; } = announcement;
    
    public override long Uin => GroupUin;

    public override string Nickname => GroupName;

    public override string Uid => GroupUin.ToString();
}
