using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity;

[Serializable]
public class OneBotGroupNoticeImage(string id, string height, string width)
{
    [JsonPropertyName("id")] public string Id { get; set; } = id;

    [JsonPropertyName("height")] public string Height { get; set; } = height;

    [JsonPropertyName("width")] public string Width { get; set; } = width;
}

[Serializable]
public class OneBotGroupNoticeMessage(string text, IEnumerable<OneBotGroupNoticeImage> images)
{
    [JsonPropertyName("text")] public string Text { get; set; } = text;

    [JsonPropertyName("images")] public IEnumerable<OneBotGroupNoticeImage> Images { get; set; } = images;
}

[Serializable]
public class OneBotGroupNotice(string noticeId, long senderId, long publishTime, OneBotGroupNoticeMessage message)
{
    [JsonPropertyName("notice_id")] public string NoticeId { get; set; } = noticeId;

    [JsonPropertyName("sender_id")] public long SenderId { get; set; } = senderId;

    [JsonPropertyName("publish_time")] public long PublishTime { get; set; } = publishTime;

    [JsonPropertyName("message")] public OneBotGroupNoticeMessage Message { get; set; } = message;
}
