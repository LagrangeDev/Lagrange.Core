using System.Text.Json.Serialization;

[Serializable]
public class OneBotFriendStatus
{
    [JsonPropertyName("status_id")] public uint StatusId { get; set; }

    [JsonPropertyName("face_id")] public uint? FaceId { get; set; }

    [JsonPropertyName("message")] public string? Message { get; set; }
}