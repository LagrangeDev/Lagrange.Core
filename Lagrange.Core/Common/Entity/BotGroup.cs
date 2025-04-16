namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotGroup
{
    internal BotGroup(uint groupUin, string groupName, string? groupRemark, uint memberCount, uint maxMember, uint createTime, string? description, string? question, string? announcement)
    {
        GroupUin = groupUin;
        GroupName = groupName;
        GroupRemark = groupRemark;
        MemberCount = memberCount;
        MaxMember = maxMember;
        CreateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(createTime);
        Description = description;
        Question = question;
        Announcement = announcement;
    }
    
    public uint GroupUin { get; }
    
    public string GroupName { get; }

    public string? GroupRemark { get; }

    public uint MemberCount { get; }
    
    public uint MaxMember { get; }
    
    public DateTime CreateTime { get; }
    
    public string? Description { get; }
    
    public string? Question { get; }
    
    public string? Announcement { get; }

    public string Avatar => $"https://p.qlogo.cn/gh/{GroupUin}/{GroupUin}/0/";
}