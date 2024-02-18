using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_group_member_info")]
public class GetGroupMemberInfoOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetGroupMemberInfo>() is { } message)
        {
            var result = (await context.FetchMembers(message.GroupId, message.NoCache)).FirstOrDefault(x => x.Uin == message.UserId);

            return result == null 
                ? new OneBotResult(null, -1, "failed") 
                : new OneBotResult(new OneBotGroupMember(message.GroupId, 
                    result.Uin,
                    result.Permission.ToString().ToLower(), 
                    result.GroupLevel.ToString(), result.MemberCard, result.MemberName, 
                    (uint)new DateTimeOffset(result.JoinTime).ToUnixTimeSeconds(), 
                    (uint)new DateTimeOffset(result.LastMsgTime).ToUnixTimeSeconds()), 
                    0, "ok");
        }

        throw new Exception();
    }
}