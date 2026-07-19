using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Converters;
using Lagrange.Milky.Models;

namespace Lagrange.Milky.Api.Handlers.System;

[ApiHandler("get_group_member_info")]
public sealed class GetGroupMemberInfoHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetGroupMemberInfoHandler.Request, GetGroupMemberInfoHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var member = (await _lagrange.FetchMembers(request.GroupId, request.NoCache).WaitAsync(ct))
            .FirstOrDefault(m => m.Uin == request.UserId);
        return member == null
            ? new MilkyApiResponse<Result>(-404, "Group member not found")
            : new MilkyApiResponse<Result>(new Result
            {
                Member = _converter.ToGroupMember(member)
            });
    }

    public sealed class Request(long groupId, long userId, bool noCache = false)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("user_id")] public required long UserId { get; init; } = userId;
        [JsonPropertyName("no_cache")] public bool NoCache { get; init; } = noCache;
    }

    public sealed class Result
    {
        [JsonPropertyName("member")] public required GroupMember Member { get; init; }
    }
}
