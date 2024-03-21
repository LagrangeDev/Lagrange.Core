using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotDeleteGroupMemo
{
    [JsonPropertyName("group_id")] public long GroupId { get; set; }

    [JsonPropertyName("notice_id")] public string NoticeId { get; set; } = string.Empty;
}
