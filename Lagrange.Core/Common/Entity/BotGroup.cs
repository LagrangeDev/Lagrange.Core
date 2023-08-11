namespace Lagrange.Core.Common.Entity;

public class BotGroup
{
    internal BotGroup(uint groupUin, string groupName)
    {
        GroupUin = groupUin;
        GroupName = groupName;
    }
    
    public uint GroupUin { get; }
    
    public string GroupName { get; }
}