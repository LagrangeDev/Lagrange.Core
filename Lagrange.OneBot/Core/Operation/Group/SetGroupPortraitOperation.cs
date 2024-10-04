using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Message.Entity;

namespace Lagrange.OneBot.Core.Operation.Group;

[Operation("set_group_portrait")]
public class SetGroupPortraitOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSetGroupPortrait>(SerializerOptions.DefaultOptions) is { } portrait)
        {
            var image = CommonResolver.ResolveStream(portrait.File);
            if (image == null) throw new Exception();
            
            var imageEntity = new ImageEntity(image);
            bool result = await context.GroupSetAvatar(portrait.GroupId, imageEntity);
            return new OneBotResult(null, result ? 0 : 1, "");
        }

        throw new Exception();
    }
}