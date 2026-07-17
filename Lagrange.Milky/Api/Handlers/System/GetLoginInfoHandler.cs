using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;

namespace Lagrange.Milky.Api.Handlers.System;

public class GetLoginInfoHandler(BotContext lagrange) : INoRequestApiHandler<GetLoginInfoHandler.Response>
{
    private readonly BotContext _lagrange = lagrange;

    public ValueTask<MilkyApiResponse<Response>> HandleAsync(CancellationToken ct) => ValueTask.FromResult(MilkyApiResponse<Response>.Ok(new Response
    {
        Uin = _lagrange.BotUin,
        Nickname = _lagrange.BotInfo?.Name ?? throw new InvalidOperationException("Bot account info is unavailable")
    }));

    public class Response
    {
        [JsonPropertyName("uin")] public required long Uin { get; init; }
        [JsonPropertyName("nickname")] public required string Nickname { get; init; }
    }
}
