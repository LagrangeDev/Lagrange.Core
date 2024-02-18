using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Entity;
using Lagrange.OneBot.Core.Entity.Action;

namespace Lagrange.OneBot.Core.Operation.Info;

[Operation("get_group_member_list")]
public class GetGroupMemberListOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotGetGroupMemberInfo>() is { } message)
        {
            var result = await context.FetchMembers(message.GroupId, message.NoCache);
            return new OneBotResult(result.Select(x => new OneBotGroupMember(message.GroupId, x.Uin, x.Permission.ToString().ToLower(), x.GroupLevel.ToString(), x.MemberCard, x.MemberName, (uint)new DateTimeOffset(x.JoinTime).ToUnixTimeSeconds(), (uint)new DateTimeOffset(x.LastMsgTime).ToUnixTimeSeconds())), 0, "ok");
        }

        throw new Exception();
    }
}