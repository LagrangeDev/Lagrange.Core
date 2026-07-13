using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models.Events;

public class GroupNudgeEventData
{
    [JsonPropertyName("group_id")] public required long GroupId { get; init; }
    [JsonPropertyName("sender_id")] public required long SenderId { get; init; }
    [JsonPropertyName("receiver_id")] public required long ReceiverId { get; init; }
    [JsonPropertyName("display_action")] public required string DisplayAction { get; init; }
    [JsonPropertyName("display_suffix")] public required string DisplaySuffix { get; init; }
    [JsonPropertyName("display_action_img_url")] public required string DisplayActionImgUrl { get; init; }
}