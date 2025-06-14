using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Message;

/// <summary>
/// 设置消息表态操作
/// 兼容 NapCat 的 set_msg_emoji_like 接口
/// </summary>
[Operation("set_msg_emoji_like")]
public class SetMsgEmojiLikeOperation(RealmHelper realm) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSetMsgEmojiLike>(SerializerOptions.DefaultOptions) is { } data)
        {
            try
            {
                // 通过消息ID获取消息记录，找到对应的序列号和群组信息
                var messageRecord = realm.Do(realm => realm.All<MessageRecord>()
                    .FirstOrDefault(record => record.Id == data.MessageId));

                if (messageRecord == null)
                {
                    return new OneBotResult(null, 1, "msg not found");
                }

                if (data.EmojiId <= 0)
                {
                    return new OneBotResult(null, 1, "emojiId not found");
                }

                // 直接尝试群聊消息表态（参考 SetGroupReactionOperation 的实现）
                // NapCat 的 set_msg_emoji_like 主要用于群聊消息表态
                bool result = await context.GroupSetMessageReaction(
                    (uint)messageRecord.ToUin,
                    (uint)messageRecord.Sequence,
                    data.EmojiId.ToString(),
                    data.Set
                );
                
                return new OneBotResult(null, result ? 0 : 1, result ? "ok" : "failed");
            }
            catch (Exception ex)
            {
                return new OneBotResult(null, 1, $"Error: {ex.Message}");
            }
        }

        throw new Exception("Invalid payload for set_msg_emoji_like operation");
    }
}
