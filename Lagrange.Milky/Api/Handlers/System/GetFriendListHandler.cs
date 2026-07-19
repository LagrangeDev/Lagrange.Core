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

[ApiHandler("get_friend_list")]
public class GetFriendListHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetFriendListHandler.Request, GetFriendListHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var friends = await _lagrange.FetchFriends(request.NoCache);
        return MilkyApiResponse<Result>.Ok(new Result
        {
            Friends = [.. friends.Select(_converter.ToFriend)]
        });
    }

    public class Request(bool noCache = false)
    {
        [JsonPropertyName("no_cache")] public bool NoCache { get; init; } = noCache;
    }

    public class Result
    {
        [JsonPropertyName("friends")] public required IReadOnlyList<Friend> Friends { get; init; }
    }
}