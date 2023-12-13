using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotStranger(uint uin, string memberName)
{
    [JsonPropertyName("user_id")] public uint Uin { get; set; } = uin;

    [JsonPropertyName("nickname")] public string MemberName { get; set; } = memberName;

    [JsonPropertyName("sex")] public string Sex { get; set; } = "";

    [JsonPropertyName("age")] public int Age { get; set; } = 0;
}