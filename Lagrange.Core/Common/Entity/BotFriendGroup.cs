namespace Lagrange.Core.Common.Entity;

public struct BotFriendGroup
{
    public uint GroupId { get; }

    public string GroupName { get; }

    internal BotFriendGroup(uint groupId, string groupName)
    {
        GroupId = groupId;
        GroupName = groupName;
    }
}