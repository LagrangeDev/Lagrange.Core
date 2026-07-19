using System.Collections.Generic;
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

[ApiHandler("get_group_member_list")]
public sealed class GetGroupMemberListHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetGroupMemberListHandler.Request, GetGroupMemberListHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var members = await _lagrange.FetchMembers(request.GroupId, request.NoCache).WaitAsync(ct);
        return new MilkyApiResponse<Result>(new Result
        {
            Members = [.. members.Select(_converter.ToGroupMember)]
        });
    }

    public sealed class Request(long groupId, bool noCache = false)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("no_cache")] public bool NoCache { get; init; } = noCache;
    }

    public sealed class Result
    {
        [JsonPropertyName("members")] public required IReadOnlyList<GroupMember> Members { get; init; }
    }
}
