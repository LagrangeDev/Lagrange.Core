using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Generic;


[Operation(".join_group_emoji_chain")]
public class GroupJoinEmojiChainOperation(RealmHelper realm) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGroupJoinEmojiChain>(SerializerOptions.DefaultOptions) is { } data)
        {
            var sequence = realm.Do(realm => realm.All<MessageRecord>()
                .First(record => record.Id == data.MessageId)
                .Sequence);

            bool res = await context.GroupJoinEmojiChain(data.GroupId, data.EmojiId, (uint)sequence);
            return new OneBotResult(null, res ? 0 : -1, res ? "ok" : "failed");
        }
        throw new Exception();
    }
}