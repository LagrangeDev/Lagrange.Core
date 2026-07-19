using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Converters;

namespace Lagrange.Milky.Api.Handlers.System;

[ApiHandler("get_friend_info")]
public sealed class GetFriendInfoHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetFriendInfoHandler.Request, GetFriendInfoHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var friend = (await _lagrange.FetchFriends(request.NoCache).WaitAsync(ct))
            .FirstOrDefault(f => f.Uin == request.UserId);

        return friend == null
            ? new MilkyApiResponse<Result>(-404, "Firend not found")
            : new MilkyApiResponse<Result>(new Result
            {
                Friend = _converter.ToFriend(friend)
            });
    }

    public sealed class Request(long userId, bool noCache = false)
    {
        [JsonPropertyName("user_id")] public required long UserId { get; init; } = userId;
        [JsonPropertyName("no_cache")] public bool NoCache { get; init; } = noCache;
    }

    public sealed class Result
    {
        [JsonPropertyName("friend")] public required Models.Friend Friend { get; init; }
    }
}
