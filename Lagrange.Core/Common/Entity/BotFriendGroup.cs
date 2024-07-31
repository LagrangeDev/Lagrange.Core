namespace Lagrange.Core.Common.Entity;

public class BotFriendGroup
{
    public uint GroupId { get; }

    public string GroupName { get; }

    internal BotFriendGroup()
    {
        GroupId = 0;
        GroupName = string.Empty;
    }

    internal BotFriendGroup(uint groupId, string groupName)
    {
        GroupId = groupId;
        GroupName = groupName;
    }
}