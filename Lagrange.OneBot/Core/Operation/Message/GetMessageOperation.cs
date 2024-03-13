using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
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
            var sender = chain.GroupMemberInfo == null
                ? new OneBotSender(chain.GroupMemberInfo?.Uin ?? 0, chain.GroupMemberInfo?.MemberName ?? string.Empty)
                : new OneBotSender(chain.FriendUin, chain.FriendInfo?.Nickname ?? string.Empty);
            var elements = service.Convert(chain);
            var response = new OneBotGetMessageResponse(chain.Time, chain.IsGroup ? "group" : "private", record.MessageHash, sender, elements);
            
            return Task.FromResult(new OneBotResult(response, 0, "ok"));
        }
        
        throw new Exception();
    }
}
