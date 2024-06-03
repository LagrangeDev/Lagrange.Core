namespace Lagrange.Core.Common.Entity;

public class BotGroup
{
    internal BotGroup(uint groupUin, string groupName, uint memberCount, uint maxMember)
    {
        GroupUin = groupUin;
        GroupName = groupName;
        MemberCount = memberCount;
        MaxMember = maxMember;
    }
    
    public uint GroupUin { get; }
    
    public string GroupName { get; }
    
    public uint MemberCount { get; }
    
    public uint MaxMember { get; }

    public string Avatar => $"https://p.qlogo.cn/gh/{GroupUin}/{GroupUin}/0/";
}