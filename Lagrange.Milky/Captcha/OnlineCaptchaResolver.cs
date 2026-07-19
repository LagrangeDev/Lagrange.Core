using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Milky.Configurations;
using Lagrange.Milky.Extensions;
using Microsoft.Extensions.Logging;
using Net.Codecrete.QrCodeGenerator;

namespace Lagrange.Milky.Captcha;

public class OnlineCaptchaResolver(ILogger<OnlineCaptchaResolver> logger, LagrangeConfiguration configuration) : ICaptchaResolver
{
    private const string Url = "https://captcha.lagrangecore.org/?{0}";
    private const string QueryUrl = "https://backend.captcha.lagrangecore.org/get_captcha?uin={0}";

    private readonly ILogger<OnlineCaptchaResolver> _logger = logger;

    private readonly long _uin = configuration.Login.Uin;
    private readonly bool _compatibleQrCode = configuration.QrCode.Compatible;

    private readonly HttpClient _http = new();

    public async Task<(string Ticket, string Randstr)> ResolveCaptchaAsync(string url, CancellationToken ct = default)
    {
        string queryString = url[(url.IndexOf('?') + 1)..].Replace("uin=0", $"uin={_uin}", StringComparison.Ordinal);
        string solveUrl = string.Format(Url, queryString);

        _logger.LogQrCode(QrCode.EncodeText(solveUrl, QrCode.Ecc.Low).ToAscii(_compatibleQrCode));
        _logger.LogCaptchaUrl(solveUrl);

        string queryUrl = string.Format(QueryUrl, _uin);
        while (true)
        {
            ct.ThrowIfCancellationRequested();

            using var response = await _http.GetAsync(queryUrl, ct);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWaitingResponse();
                await Task.Delay(TimeSpan.FromSeconds(1), ct);
                continue;
            }

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

            if (!document.RootElement.TryGetProperty("data", out var dataElement))
            {
                _logger.LogWaitingResponse();
                await Task.Delay(TimeSpan.FromSeconds(1), ct);
                continue;
            }
            if (dataElement.ValueKind != JsonValueKind.String)
            {
                _logger.LogWaitingResponse();
                await Task.Delay(TimeSpan.FromSeconds(1), ct);
                continue;
            }

            string? json = dataElement.GetString();
            if (json == null)
            {
                _logger.LogWaitingResponse();
                await Task.Delay(TimeSpan.FromSeconds(1), ct);
                continue;
            }

            int sep = json.IndexOf('|');
            if (sep <= 0 || sep >= json.Length - 1)
            {
                _logger.LogWaitingResponse();
                await Task.Delay(TimeSpan.FromSeconds(1), ct);
                continue;
            }

            _logger.LogCaptchaResolved();
            return (json[..sep], json[(sep + 1)..]);
        }
    }
}

public static partial class OnlineCaptchaResolverLoggerExtension
{
    [LoggerMessage(LogLevel.Information, "\n{QrCode}")]
    public static partial void LogQrCode(this ILogger<OnlineCaptchaResolver> logger, string qrcode);

    [LoggerMessage(LogLevel.Information, "Please scan the QR code or open the following link to complete captcha: {Url}")]
    public static partial void LogCaptchaUrl(this ILogger<OnlineCaptchaResolver> logger, string url);

    [LoggerMessage(LogLevel.Debug, "Waiting for captcha response...")]
    public static partial void LogWaitingResponse(this ILogger<OnlineCaptchaResolver> logger);

    [LoggerMessage(LogLevel.Information, "Captcha solved successfully")]
    public static partial void LogCaptchaResolved(this ILogger<OnlineCaptchaResolver> logger);
}