using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotGroupMsgHistoryResponse(List<OneBotGroupMsg> messages)
{
    [JsonPropertyName("messages")] public List<OneBotGroupMsg> Messages { get; set; } = messages;
}