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
            using var image = await CommonResolver.ResolveStreamAsync(portrait);
            if (image != null)
            {
                var imageEntity = new ImageEntity(image);
                bool result = await context.SetAvatar(imageEntity);
                return new OneBotResult(null, result ? 0 : 1, result ? "ok" : "failed");
            }
        }

        throw new Exception("Failed to resolve avatar image");
    }
}