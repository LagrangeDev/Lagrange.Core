using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotGroupReaction(uint selfId, uint targetGroupUin, int targetMessage, uint operatorId, string subType, string code)
    : OneBotNotify(selfId, "reaction")
{
    [JsonPropertyName("target_group_uin")] public uint TargetGroupUin { get; } = targetGroupUin;

    [JsonPropertyName("target_message")] public int TargetMessage { get; } = targetMessage;

    [JsonPropertyName("operator_id")] public uint OperatorId { get; } = operatorId;

    [JsonPropertyName("sub_type")] public string SubType { get; } = subType;

    [JsonPropertyName("code")] public string Code { get; } = code;
}