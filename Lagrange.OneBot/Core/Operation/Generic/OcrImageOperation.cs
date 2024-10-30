using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Message.Entity;

namespace Lagrange.OneBot.Core.Operation.Generic;


[Operation(".ocr_image")]
[Operation("ocr_image")]
public class OcrImageOperation() : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotOcrImage>(SerializerOptions.DefaultOptions) is { } data && CommonResolver.ResolveStream(data.Image) is { } stream)
        {
            var entity = new ImageEntity(stream);
            var res = await context.OcrImage(entity);
            return new OneBotResult(res, 0, "ok");
        }
        throw new Exception();
    }
}