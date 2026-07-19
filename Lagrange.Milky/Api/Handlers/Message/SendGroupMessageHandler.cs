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

[ApiHandler("send_group_message")]
public sealed class SendGroupMessageHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<SendGroupMessageHandler.Request, SendGroupMessageHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var chain = await _converter.FromOutgoingSegmentsAsync(request.Message, MessageType.Group, request.GroupId, ct);
        var message = await _lagrange.SendGroupMessage(request.GroupId, chain).WaitAsync(ct);
        return new MilkyApiResponse<Result>(new Result
        {
            MessageSeq = (long)message.Sequence,
            Time = message.Time.ToUnixTimeSeconds(),
        });
    }

    public sealed class Request(long groupId, IReadOnlyList<OutgoingSegmentBase> message)
    {
        [JsonPropertyName("group_id")] public required long GroupId { get; init; } = groupId;
        [JsonPropertyName("message")] public required IReadOnlyList<OutgoingSegmentBase> Message { get; init; } = message;
    }

    public sealed class Result
    {
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; }
        [JsonPropertyName("time")] public required long Time { get; init; }
    }
}
