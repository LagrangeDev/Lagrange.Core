using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Converters;

namespace Lagrange.Milky.Api.Handlers.System;

[ApiHandler("get_group_info")]
public sealed class GetGroupInfoHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetGroupInfoHandler.Request, GetGroupInfoHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var group = (await _lagrange.FetchGroups(request.NoCache).WaitAsync(ct))
            .FirstOrDefault(g => g.Uin == request.GroupId);

        return group == null
            ? new MilkyApiResponse<Result>(-404, "Group not found")
            : new MilkyApiResponse<Result>(new Result
            {
                Group = _converter.ToGroup(group)
            });
    }

    public sealed class Request(long groupId, bool noCache = false)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("no_cache")] public bool NoCache { get; init; } = noCache;
    }

    public sealed class Result
    {
        [JsonPropertyName("group")] public required Models.Group Group { get; init; }
    }
}
