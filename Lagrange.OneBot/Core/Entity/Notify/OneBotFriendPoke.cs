using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public class OneBotFriendPoke(uint selfId) : OneBotNotify(selfId, "notify")
{
    [JsonPropertyName("sub_type")] public string SubType { get; set; } = "poke";

    [JsonPropertyName("sender_id")] public uint SenderId { get; set; }

    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("target_id")] public uint TargetId { get; set; }

    [JsonPropertyName("action")] public string Action { get; set; } = string.Empty;

    [JsonPropertyName("suffix")] public string Suffix { get; set; } = string.Empty;
  
    [JsonPropertyName("action_img_url")] public string ActionImgUrl { get; set; } = string.Empty;
}
