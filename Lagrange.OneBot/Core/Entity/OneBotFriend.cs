using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotFriend
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; }

    [JsonPropertyName("nickname")] public string NickName { get; set; } = "";

    [JsonPropertyName("remark")] public string Remark { get; set; } = "";

    [JsonPropertyName("group_id")] public uint GroupId { get; set; } = 0;

    [JsonPropertyName("group_name")] public string GroupName { get; set; } = "";
}