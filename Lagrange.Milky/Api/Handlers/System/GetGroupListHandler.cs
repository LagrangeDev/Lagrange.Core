using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Converters;

namespace Lagrange.Milky.Api.Handlers.System;

[ApiHandler("get_group_list")]
public sealed class GetGroupListHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetGroupListHandler.Request, GetGroupListHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var groups = await _lagrange.FetchGroups(request.NoCache).WaitAsync(ct);
        return new MilkyApiResponse<Result>(new Result
        {
            Groups = [.. groups.Select(_converter.ToGroup)]
        });
    }

    public sealed class Request(bool noCache = false)
    {
        [JsonPropertyName("no_cache")] public bool NoCache { get; init; } = noCache;
    }

    public sealed class Result
    {
        [JsonPropertyName("groups")] public required IReadOnlyList<Models.Group> Groups { get; init; }
    }
}
