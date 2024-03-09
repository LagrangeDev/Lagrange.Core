using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotFriendMsgHistoryResponse(List<object> messages)
{
    [JsonPropertyName("messages")] public List<object> Messages { get; set; } = messages;
}