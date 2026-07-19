using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Lagrange.Milky.Captcha;

public class ManualCaptchaResolver(ILogger<ManualCaptchaResolver> logger) : ICaptchaResolver
{
    private readonly ILogger<ManualCaptchaResolver> _logger = logger;

    public async Task<(string Ticket, string Randstr)> ResolveCaptchaAsync(string url, CancellationToken ct = default)
    {
        _logger.LogCaptchUrl(url);

        _logger.LogTicketInputPrompt();
        string ticket = await Console.In.ReadLineAsync(ct) ?? string.Empty;

        _logger.LogRandstrInputPrompt();
        string randstr = await Console.In.ReadLineAsync(ct) ?? string.Empty;

        return (ticket, randstr);
    }
}

public static partial class ManualCaptchaResolverLoggerExtension
{
    [LoggerMessage(LogLevel.Information, "Captcha URL: {Url}")]
    public static partial void LogCaptchUrl(this ILogger<ManualCaptchaResolver> logger, string url);

    [LoggerMessage(LogLevel.Information, "Please enter the ticket: ")]
    public static partial void LogTicketInputPrompt(this ILogger<ManualCaptchaResolver> logger);

    [LoggerMessage(LogLevel.Information, "Please enter the randstr: ")]
    public static partial void LogRandstrInputPrompt(this ILogger<ManualCaptchaResolver> logger);
}