using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotFriendRequest(uint selfId, uint userId) : OneBotNotify(selfId, "friend_add")
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;
}