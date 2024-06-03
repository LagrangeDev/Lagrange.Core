using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotMemberDecrease(uint selfId, string subType, uint groupId, uint operatorId, uint userId) 
    : OneBotNotify(selfId, "group_decrease")
{
    [JsonPropertyName("sub_type")] public string SubType { get; set; } = subType;

    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = groupId;

    [JsonPropertyName("operator_id")] public uint OperatorId { get; set; } = operatorId;

    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;
}