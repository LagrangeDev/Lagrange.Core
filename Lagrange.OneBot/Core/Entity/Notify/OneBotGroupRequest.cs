using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotGroupRequest(uint selfId, uint userId, uint groupId, string subType, string flag) 
    : OneBotRequest(selfId, "group", "", flag)
{
    [JsonPropertyName("sub_type")] public string SubType { get; set; } = subType;
    
    [JsonPropertyName("group_id")] public uint UserId { get; set; } = userId;

    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = groupId;
}