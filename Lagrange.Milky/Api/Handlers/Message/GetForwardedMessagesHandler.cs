using System.Collections.Generic;
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

[ApiHandler("get_forwarded_messages")]
public sealed class GetForwardedMessagesHandler(BotContext lagrange, MilkyConverter converter) : IApiHandler<GetForwardedMessagesHandler.Request, GetForwardedMessagesHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MilkyConverter _converter = converter;

    public async ValueTask<MilkyApiResponse<Result>> HandleAsync(Request request, CancellationToken ct)
    {
        var messages = await _lagrange.GetForwardedMessages(request.ForwardId).WaitAsync(ct);

        var result = new List<IncomingForwardedMessage>(messages.Count);
        foreach (var message in messages) result.Add(await _converter.ToForwardedMessageAsync(message, ct));

        return new MilkyApiResponse<Result>(new Result { Messages = result });
    }

    public sealed class Request(string forwardId)
    {
        [JsonPropertyName("forward_id")] public required string ForwardId { get; init; } = forwardId;
    }

    public sealed class Result
    {
        [JsonPropertyName("messages")] public required IReadOnlyList<IncomingForwardedMessage> Messages { get; init; }
    }
}
