using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Database;
using Lagrange.OneBot.Message;
using LiteDB;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_msg")]
public class GetMessageOperation(LiteDatabase database, MessageService service) : IOperation
{
    public Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetMessage>(SerializerOptions.DefaultOptions) is { } getMsg)
        {
            var record = database.GetCollection<MessageRecord>().FindById(getMsg.MessageId);
            var chain = (MessageChain)record;

            OneBotSender sender = chain switch
            {
                { GroupMemberInfo: BotGroupMember g } => new(g.Uin, g.MemberName),
                { FriendInfo: BotFriend f } => new(f.Uin, f.Nickname),
                _ => new(chain.FriendUin, "")
            };

            var elements = service.Convert(chain);
            var response = new OneBotGetMessageResponse(chain.Time, chain.IsGroup ? "group" : "private", record.MessageHash, sender, elements);

            return Task.FromResult(new OneBotResult(response, 0, "ok"));
        }

        throw new Exception();
    }
}
