using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action.Response;

[Serializable]
public class OneBotSetGroupMemoResponse(string noticeId)
{
    [JsonPropertyName("notice_id")] public string NoticeId { get; set; } = noticeId;
}
