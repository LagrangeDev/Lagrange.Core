using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotFriendRecall(uint selfId) : OneBotNotify(selfId, "friend_recall")
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("message_id")] public int MessageId { get; set; }
}