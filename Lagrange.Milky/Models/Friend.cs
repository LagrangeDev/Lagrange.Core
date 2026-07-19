using System.Text.Json.Serialization;

namespace Lagrange.Milky.Models;

public class Friend
{
    [JsonPropertyName("user_id")] public required long UserId { get; init; }
    [JsonPropertyName("nickname")] public required string Nickname { get; init; }
    [JsonPropertyName("sex")] public required string Sex { get; init; }
    [JsonPropertyName("qid")] public required string Qid { get; init; }
    [JsonPropertyName("remark")] public required string Remark { get; init; }
    [JsonPropertyName("category")] public required FriendCategory Category { get; init; }
}
