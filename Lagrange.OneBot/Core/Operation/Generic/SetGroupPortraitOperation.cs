using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Message.Entity;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation("set_qq_avatar")]
public class SetQQAvatar : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload?["file"]?.ToString() is { } portrait)
        {
            var image = CommonResolver.ResolveStream(portrait);
            if (image == null) throw new Exception();
            
            var imageEntity = new ImageEntity(image);
            bool result = await context.SetAvatar(imageEntity);
            return new OneBotResult(null, result ? 0 : 1, "");
        }

        throw new Exception();
    }
}