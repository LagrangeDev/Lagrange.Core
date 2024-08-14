using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

public class OneBotFriendGroup(uint groupId, string groupName)
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = groupId;

    [JsonPropertyName("group_name")] public string GroupName { get; set; } = groupName;
}