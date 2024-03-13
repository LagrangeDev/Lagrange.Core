using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotFriendPoke(uint selfId)  : OneBotNotify(selfId, "poke")
{
    [JsonPropertyName("sender_id")] public uint SenderId { get; set; }
    
    [JsonPropertyName("user_id")] public uint UserId { get; set; }
    
    [JsonPropertyName("target_id")] public uint TargetId { get; set; } // self
}