using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;
using Lagrange.OneBot.Core.Entity.Message;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Message;

namespace Lagrange.OneBot.Core.Operation.Message;

[Operation("get_forward_msg")]
public class GetForwardMsgOperation(MessageService service) : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetForwardMsg>(SerializerOptions.DefaultOptions) is { } forwardMsg)
        {
            var (code, chains) = await context.GetMessagesByResId(forwardMsg.Id);

            if (code != 0) return new OneBotResult(null, code, "failed");

            var nodes = new List<OneBotSegment>();
            if (chains == null) return new OneBotResult(new OneBotGetForwardMsgResponse(nodes), 0, "ok");

            foreach (var chain in chains)
            {
                var parsed = service.Convert(chain);
                var nickname = string.IsNullOrEmpty(chain.FriendInfo?.Nickname)
                    ? chain.GroupMemberInfo?.MemberName
                    : chain.FriendInfo?.Nickname;
                var node = new OneBotNode(chain.FriendUin.ToString(), nickname ?? "", parsed);
                nodes.Add(new OneBotSegment("node", node));
            }

            return new OneBotResult(new OneBotGetForwardMsgResponse(nodes), 0, "ok");
        }

        throw new Exception();
    }
}
