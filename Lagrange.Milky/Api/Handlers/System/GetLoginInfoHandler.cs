using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Milky.Api.Attributes;

namespace Lagrange.Milky.Api.Handlers.System;

[ApiHandler("get_login_info")]
public sealed class GetLoginInfoHandler(BotContext lagrange) : INoRequestApiHandler<GetLoginInfoHandler.Result>
{
    private readonly BotContext _lagrange = lagrange;

    public ValueTask<MilkyApiResponse<Result>> HandleAsync(CancellationToken ct)
    {
        return ValueTask.FromResult(new MilkyApiResponse<Result>(new Result
        {
            Uin = _lagrange.BotUin,
            Nickname = _lagrange.BotInfo?.Name ?? string.Empty,
        }));
    }

    public sealed class Result
    {
        [JsonPropertyName("uin")] public required long Uin { get; init; }
        [JsonPropertyName("nickname")] public required string Nickname { get; init; }
    }
}
