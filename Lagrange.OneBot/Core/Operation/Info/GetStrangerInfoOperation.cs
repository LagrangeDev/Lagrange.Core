using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_stranger_info")]
public class GetStrangerInfoOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetStrangerInfo>(SerializerOptions.DefaultOptions) is { } stranger)
        {
            BotGroupMember? target = null;

            foreach (var group in await context.ContextCollection.Business.CachingLogic.GetCachedGroups(!stranger.NoCache))
            {
                foreach (var member in await context.ContextCollection.Business.CachingLogic.GetCachedMembers(group.GroupUin, false))
                {
                    if (stranger.UserId == member.Uin)
                    {
                        target = member;
                        break;
                    }
                }
                
                if (target != null) break;
            }

            return target != null 
                ? new OneBotResult(new OneBotStranger { UserId = target.Uin, NickName = target.MemberName }, 0, "ok") 
                : new OneBotResult(null, 1, "member not found");
        }

        throw new Exception();
    }
}