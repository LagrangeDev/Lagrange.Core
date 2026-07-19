using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Message;
using Lagrange.Milky.Api.Attributes;
using Lagrange.Milky.Caching;
using Lagrange.Milky.Converters;
using Lagrange.Milky.Models;
using Lagrange.Milky.Models.Messages;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("get_message")]
public sealed class GetMessageHandler(BotContext lagrange, MessageCache cache, MilkyConverter converter) : IApiHandler<GetMessageHandler.Request, GetMessageHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MessageCache _cache = cache;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var messageType = request.MessageScene switch
        {
            "friend" => MessageType.Private,
            "group" => MessageType.Group,
            _ => throw new NotSupportedException(),
        };
        var message = _cache.Get(messageType, request.PeerId, (ulong)request.MessageSeq)
            ?? (messageType switch
            {
                MessageType.Private => await _lagrange.GetC2CMessage(
                    request.PeerId,
                    (ulong)request.MessageSeq,
                    (ulong)request.MessageSeq
                ).WaitAsync(ct),
                MessageType.Group => await _lagrange.GetGroupMessage(
                    request.PeerId,
                    (ulong)request.MessageSeq,
                    (ulong)request.MessageSeq
                ).WaitAsync(ct),
                _ => throw new NotSupportedException(),
            }).FirstOrDefault();
        return message == null
            ? new MilkyApiResponse<Result>(-404, "Message not found")
            : new MilkyApiResponse<Result>(new Result
            {
                Message = await _converter.ToIncomingMessageAsync(message, ct)
            });
    }

    public sealed class Request(string messageScene, long peerId, long messageSeq)
    {
        [JsonPropertyName("message_scene")] public required string MessageScene { get; init; } = messageScene;
        [JsonPropertyName("peer_id")] public required long PeerId { get; init; } = peerId;
        [JsonPropertyName("message_seq")] public required long MessageSeq { get; init; } = messageSeq;
    }

    public sealed class Result
    {
        [JsonPropertyName("message")] public required IncomingMessageBase Message { get; init; }
    }
}
