using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotGroupRequest(uint selfId, uint userId, uint groupId, string subType, string? comment, string flag) 
    : OneBotRequest(selfId, "group", comment, flag)
{
    [JsonPropertyName("sub_type")] public string SubType { get; set; } = subType;
    
    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;

    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = groupId;
    
    [JsonPropertyName("invitor_id")] public uint InvitorId { get; set; }
}