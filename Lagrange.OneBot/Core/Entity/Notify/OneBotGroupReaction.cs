using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotGroupReaction(uint selfId, uint groupId, int messageId, uint operatorId, string subType, string code, uint count)
    : OneBotNotify(selfId, "reaction")
{
    [JsonPropertyName("group_id")] public uint GroupId { get; } = groupId;

    [JsonPropertyName("message_id")] public int MessageId { get; } = messageId;

    [JsonPropertyName("operator_id")] public uint OperatorId { get; } = operatorId;

    [JsonPropertyName("sub_type")] public string SubType { get; } = subType;

    [JsonPropertyName("code")] public string Code { get; } = code;


    [JsonPropertyName("count")] public uint Count { get; } = count;
}