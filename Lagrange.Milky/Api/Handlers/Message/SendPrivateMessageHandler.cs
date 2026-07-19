using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Message;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Converters;
using Lagrange.Milky.Extensions;
using Lagrange.Milky.Models;
using Lagrange.Milky.Models.Segments;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("send_private_message")]
public sealed class SendPrivateMessageHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<SendPrivateMessageHandler.Request, SendPrivateMessageHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var chain = await _converter.FromOutgoingSegmentsAsync(request.Message, MessageType.Private, request.UserId, ct);
        var message = await _lagrange.SendFriendMessage(request.UserId, chain).WaitAsync(ct);
        return new MilkyApiResponse<Result>(new Result
        {
            MessageSeq = (long)message.ClientSequence,
            Time = message.Time.ToUnixTimeSeconds(),
        });
    }

    public sealed class Request(long userId, IReadOnlyList<OutgoingSegmentBase> message)
    {
        [JsonPropertyName("user_id")] public required long UserId { get; init; } = userId;
        [JsonPropertyName("message")] public required IReadOnlyList<OutgoingSegmentBase> Message { get; init; } = message;
    }

    public sealed class Result
    {
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
        [JsonPropertyName("time")] public required long Time { get; init; }
    }
}
