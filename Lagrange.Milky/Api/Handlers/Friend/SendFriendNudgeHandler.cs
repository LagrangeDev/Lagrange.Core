using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.Friend;

[ApiHandler("send_friend_nudge")]
public sealed class SendFriendNudgeHandler(BotContext lagrange) : INoResultApiHandler<SendFriendNudgeHandler.Request>
{
    private readonly BotContext _lagrange = lagrange;

    public async ValueTask<MilkyApiResponse> HandleAsync(Request request, CancellationToken ct)
    {
        await _lagrange.SendFriendNudge(
            request.UserId,
            request.IsSelf ? _lagrange.BotUin : request.UserId
        ).WaitAsync(ct);
        return new MilkyApiResponse();
    }

    public sealed class Request(long userId, bool isSelf = false)
    {
        [JsonPropertyName("user_id")] public required long UserId { get; init; } = userId;
        [JsonPropertyName("is_self")] public bool IsSelf { get; init; } = isSelf;
    }
}
