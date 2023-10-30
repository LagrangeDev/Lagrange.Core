using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Notify;

[Serializable]
public abstract class OneBotNotify(uint selfId, string noticeType) : OneBotEntityBase(selfId, "notice")
{
    [JsonPropertyName("notice_type")] public string NoticeType { get; set; } = noticeType;
}