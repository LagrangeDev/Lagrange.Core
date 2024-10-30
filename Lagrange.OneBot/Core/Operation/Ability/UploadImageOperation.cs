using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Message.Entity;

namespace Lagrange.OneBot.Core.Operation.Ability;

[Operation("upload_image")]
public class UploadImageOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload?["file"]?.ToString() is { } file && CommonResolver.ResolveStream(file) is { } stream)
        {
            var entity = new ImageEntity(stream);
            var url = await context.UploadImage(entity);
            return new OneBotResult(url, 0, "ok");
        }

        throw new Exception();
    }
}