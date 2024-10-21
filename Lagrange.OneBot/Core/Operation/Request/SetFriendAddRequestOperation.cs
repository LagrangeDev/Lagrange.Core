using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Request;

[Operation("set_friend_add_request")]
public class SetFriendAddRequestOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSetRequest>(SerializerOptions.DefaultOptions) is { } request)
        {
            bool result = await context.ContextCollection.Business.OperationLogic.SetFriendRequest(request.Flag, request.Approve);
            return new OneBotResult(null, result ? 0 : 1, "ok");
        }

        throw new Exception();
    }
}