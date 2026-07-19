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

[ApiHandler("get_friend_list")]
public sealed class GetFriendListHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetFriendListHandler.Request, GetFriendListHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var friends = await _lagrange.FetchFriends(request.NoCache).WaitAsync(ct);
        return new MilkyApiResponse<Result>(new Result
        {
            Friends = [.. friends.Select(_converter.ToFriend)]
        });
    }

    public sealed class Request(bool noCache = false)
    {
        [JsonPropertyName("no_cache")] public bool NoCache { get; init; } = noCache;
    }

    public sealed class Result
    {
        [JsonPropertyName("friends")] public required IReadOnlyList<Models.Friend> Friends { get; init; }
    }
}
