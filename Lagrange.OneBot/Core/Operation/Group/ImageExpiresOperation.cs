using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("image_expires")]
public class ImageExpiresOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        var message = payload.Deserialize<OneBotImageExpires>(SerializerOptions.DefaultOptions);

        if (message != null)
        {
            bool _ = await context.ImageExpires(message.Url);

            return new OneBotResult(message.Url, 0, "ok");
        }

        throw new Exception();
    }
}
