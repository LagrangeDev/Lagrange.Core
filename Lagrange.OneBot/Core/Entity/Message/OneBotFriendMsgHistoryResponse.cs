using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotFriendMsgHistoryResponse(List<OneBotPrivateMsg> messages)
{
    [JsonPropertyName("messages")] public List<OneBotPrivateMsg> Messages { get; set; } = messages;
}