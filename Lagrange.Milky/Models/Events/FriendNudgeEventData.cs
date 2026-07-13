using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

// TODO: friend nudeg event is not implemented in core
public class FriendNudgeEventData
{
    [JsonPropertyName("user_id")] public required long UserId { get; init; }
    [JsonPropertyName("is_self_send")] public required bool IsSelfSend { get; init; }
    [JsonPropertyName("is_self_receive")] public required bool IsSelfReceive { get; init; }
    [JsonPropertyName("display_action")] public required string DisplayAction { get; init; }
    [JsonPropertyName("display_suffix")] public required string DisplaySuffix { get; init; }
    [JsonPropertyName("display_action_img_url")] public required string DisplayActionImgUrl { get; init; }
}