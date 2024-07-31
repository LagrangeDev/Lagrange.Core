using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

public class OneBotFriendGroup
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = 0;

    [JsonPropertyName("group_name")] public string GroupName { get; set; } = "";
}