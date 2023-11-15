using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotGroupAdmin(uint selfId, string subType, uint groupId, uint userId) : OneBotNotify(selfId, "group_admin")
{
    [JsonPropertyName("sub_type")] public string SubType { get; set; } = subType;

    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = groupId;

    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;
}