using System;
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
using Lagrange.Milky.Models.Messages;

namespace Lagrange.Milky.Api.Handlers.Message;

[ApiHandler("get_history_messages")]
public sealed class GetHistoryMessagesHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetHistoryMessagesHandler.Request, GetHistoryMessagesHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;


    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        long endSequence = request.StartMessageSeq ?? request.MessageScene switch
        {
            "friend" => throw new NotSupportedException(), // TODO: core cannot retrieve the latest sequence in a friend scene.
            "group" => (await _lagrange.FetchGroupExtra(request.PeerId).WaitAsync(ct)).LatestMessageSequence,
            _ => throw new NotSupportedException(),
        };

        var messages = request.MessageScene switch
        {
            "friend" => await _lagrange.GetC2CMessage(
                request.PeerId,
                (ulong)Math.Max(0, endSequence - request.Limit),
                (ulong)endSequence
            ).WaitAsync(ct),
            "group" => await _lagrange.GetGroupMessage(
                request.PeerId,
                (ulong)Math.Max(0, endSequence - request.Limit),
                (ulong)endSequence
            ).WaitAsync(ct),
            _ => throw new NotSupportedException(),
        };

        var incomingMessages = new IncomingMessageBase[messages.Count];
        for (int i = 0; i < messages.Count; i++)
        {
            incomingMessages[i] = await _converter.ToIncomingMessageAsync(messages[i], ct);
        }

        long? nextSequence = incomingMessages.Min(m => m.MessageSeq) - 1;
        if (nextSequence.Value < 0) nextSequence = null;

        return new MilkyApiResponse<Result>(new Result
        {
            Messages = [.. incomingMessages.OrderBy(m => m.MessageSeq)],
            NextMessageSeq = nextSequence,
        });
    }

    public sealed class Request(string messageScene, long peerId, long? startMessageSeq, int limit = 20)
    {
        [JsonPropertyName("message_scene")] public required string MessageScene { get; init; } = messageScene;
        [JsonPropertyName("peer_id")] public required long PeerId { get; init; } = peerId;
        [JsonPropertyName("start_message_seq")] public long? StartMessageSeq { get; init; } = startMessageSeq;
        [JsonPropertyName("limit")] public int Limit { get; init; } = limit;
    }

    public sealed class Result
    {
        [JsonPropertyName("messages")] public required IReadOnlyList<IncomingMessageBase> Messages { get; init; }
        [JsonPropertyName("next_message_seq")] public required long? NextMessageSeq { get; init; }
    }
}
