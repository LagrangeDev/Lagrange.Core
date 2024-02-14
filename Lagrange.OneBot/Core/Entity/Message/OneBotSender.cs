using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Message;

[Serializable]
public class OneBotSender(uint userId, string nickName)
{
    [JsonPropertyName("user_id")] public uint UserId { get; set; } = userId;

    [JsonPropertyName("nickname")] public string NickName { get; set; } = nickName;

    [JsonPropertyName("sex")] public string Sex { get; set; } = "unknown";

    [JsonPropertyName("age")] public int Age = -1;
}