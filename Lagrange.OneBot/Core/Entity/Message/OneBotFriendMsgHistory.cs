using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotFriendMsgHistory
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("message_id")] public int MessageId { get; set; }

    [JsonPropertyName("count")] public uint Count { get; set; } = 20;
}