using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Operation.Converters;
using Lagrange.OneBot.Utility;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_stranger_info")]
public class GetStrangerInfoOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetStrangerInfo>(SerializerOptions.DefaultOptions) is { } stranger)
        {
            if (await context.FetchUserInfo(stranger.UserId) is { } info)
            {
                return new OneBotResult(new OneBotStranger
                {
                    UserId = info.Uin,
                    QId = info.Qid,
                    NickName = info.Nickname,
                    Sex = info.Gender.ToOneBotString(),
                    Age = info.Age,
                    Level = info.Level
                }, 0, "ok");
            }

            return new OneBotResult(null, -1, "failed");
        }
        throw new Exception();
    }
}