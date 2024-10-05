namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotGroupMember
{
    /// <summary>
    /// The empty constructor for serialization
    /// </summary>
    internal BotGroupMember()
    {
        Uid = string.Empty;
        MemberCard = string.Empty;
        MemberName = string.Empty;
        SpecialTitle = string.Empty;
    }

    internal BotGroupMember(uint uin, string uid, GroupMemberPermission permission, uint groupLevel, string? memberCard,
        string memberName, string? specialTitle, DateTime joinTime, DateTime lastMsgTime,DateTime shutUpTimestamp)
    {
        Uin = uin;
        Uid = uid;
        Permission = permission;
        GroupLevel = groupLevel;
        MemberCard = memberCard;
        MemberName = memberName;
        SpecialTitle = specialTitle;
        JoinTime = joinTime;
        LastMsgTime = lastMsgTime;
        ShutUpTimestamp = shutUpTimestamp;
    }

    public uint Uin { get; set; }

    internal string Uid { get; set; }

    public GroupMemberPermission Permission { get; set; }

    public uint GroupLevel { get; set; }

    public string? MemberCard { get; set; }

    public string MemberName { get; set; }

    public string? SpecialTitle { get; set; }

    public DateTime JoinTime { get; set; }

    public DateTime LastMsgTime { get; set; }

    public DateTime ShutUpTimestamp { get; set; }

    public string Avatar => $"https://q1.qlogo.cn/g?b=qq&nk={Uin}&s=640";
}

public enum GroupMemberPermission : uint
{
    Member = 0,
    Owner = 1,
    Admin = 2,
}