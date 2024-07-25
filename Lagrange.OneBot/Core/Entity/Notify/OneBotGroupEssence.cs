using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotGroupEssence(uint selfId) : OneBotNotify(selfId, "essence")
{
    [JsonPropertyName("sub_type")] public string SubType { get; set; } = string.Empty;

    [JsonPropertyName("group_id")] public uint GroupId { get; set; }

    [JsonPropertyName("sender_id")] public uint SenderId { get; set; }

    [JsonPropertyName("operator_id")] public uint OperatorId { get; set; }

    [JsonPropertyName("message_id")] public int MessageId { get; set; }
}
