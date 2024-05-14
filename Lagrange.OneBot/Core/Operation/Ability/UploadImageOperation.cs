using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Internal.Event.Message;
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
            await context.ContextCollection.Highway.ManualUploadEntity(entity);
            var msgInfo = entity.MsgInfo;
            if (msgInfo is null) throw new Exception();

            var downloadEvent = ImageDownloadEvent.Create(context.ContextCollection.Keystore.Uid ?? "", msgInfo);
            var result = await context.ContextCollection.Business.SendEvent(downloadEvent);
            var ret = (ImageDownloadEvent)result[0];

            return new OneBotResult(ret.ImageUrl, 0, "ok");
        }

        throw new Exception();
    }
}