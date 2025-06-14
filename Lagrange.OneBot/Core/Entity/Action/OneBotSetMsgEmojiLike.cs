using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

/// <summary>
/// 设置消息表态请求
/// 兼容 NapCat 的 set_msg_emoji_like 接口
/// </summary>
[Serializable]
public class OneBotSetMsgEmojiLike
{
    /// <summary>
    /// 消息ID
    /// </summary>
    [JsonPropertyName("message_id")] 
    public int MessageId { get; set; }

    /// <summary>
    /// 表情ID（支持数字类型）
    /// </summary>
    [JsonPropertyName("emoji_id")] 
    public int EmojiId { get; set; }

    /// <summary>
    /// 是否设置表态，默认为 true
    /// </summary>
    [JsonPropertyName("set")] 
    public bool Set { get; set; } = true;
}
