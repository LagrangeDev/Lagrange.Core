using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Events.EventArgs;
using Lagrange.Milky.Captcha;
using Lagrange.Milky.Configurations;
using Lagrange.Milky.Extensions;
using Lagrange.Milky.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Net.Codecrete.QrCodeGenerator;
using static Lagrange.Core.Events.EventArgs.BotQrCodeQueryEvent;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Lagrange.Milky.Login;

public class LoginService(IHostEnvironment environment, ILogger<LoginService> logger,LagrangeConfiguration configuration, BotContext lagrange, ICaptchaResolver captchaResolver) : IHostedService
{
    private readonly IHostEnvironment _environment = environment;
    private readonly ILogger<LoginService> _logger = logger;

    private readonly long _uin = configuration.Login.Uin;
    private readonly string? _password = configuration.Login.Password;
    private readonly bool _compatibleQrCode = configuration.QrCode.Compatible;

    private readonly BotContext _lagrange = lagrange;
    private readonly ICaptchaResolver _captchaResolver = captchaResolver;

    private CancellationTokenSource? _cts;
    private CancellationToken StoppingToken => _cts?.Token
        ?? throw new Exception("The service has not been started yet.");

    public async Task StartAsync(CancellationToken ct)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

        _lagrange.EventInvoker.RegisterEvent<BotRefreshKeystoreEvent>(OnRefreshKeystore);

        _lagrange.EventInvoker.RegisterEvent<BotQrCodeEvent>(OnQrCode);
        _lagrange.EventInvoker.RegisterEvent<BotQrCodeQueryEvent>(OnQrCodeQuery);
        _lagrange.EventInvoker.RegisterEvent<BotCaptchaEvent>(OnCaptchaAsync);
        _lagrange.EventInvoker.RegisterEvent<BotSMSEvent>(OnSms);
        _lagrange.EventInvoker.RegisterEvent<BotNewDeviceVerifyEvent>(OnNewDeviceVerify);

        bool success = await (_password != null
            ? _lagrange.Login(_uin, _password, _cts.Token)
            : _lagrange.Login(_cts.Token));

        if (!success) throw new Exception("Failed to login for account {_uin}."); // TODO
        _logger.LogLoginSuccess(_uin, _lagrange.Config.Protocol, _lagrange.AppInfo.CurrentVersion);

        _lagrange.EventInvoker.UnregisterEvent<BotQrCodeEvent>(OnQrCode);
        _lagrange.EventInvoker.UnregisterEvent<BotQrCodeQueryEvent>(OnQrCodeQuery);
        _lagrange.EventInvoker.UnregisterEvent<BotCaptchaEvent>(OnCaptchaAsync);
        _lagrange.EventInvoker.UnregisterEvent<BotSMSEvent>(OnSms);
        _lagrange.EventInvoker.UnregisterEvent<BotNewDeviceVerifyEvent>(OnNewDeviceVerify);
    }

    private async Task OnRefreshKeystore(BotContext lagrange, BotRefreshKeystoreEvent @event)
    {
        string ksPath = Path.Combine(_environment.ContentRootPath, $"{_uin}.ks");
        await File.WriteAllTextAsync(ksPath, Serializer.JsonSerialize(@event.Keystore), StoppingToken);
    }

    private async Task OnQrCode(BotContext lagrange, BotQrCodeEvent @event)
    {
        using var stream = new FileStream(
            "qrcode.png",
            FileMode.Create,
            FileAccess.Write,
            FileShare.ReadWrite,
            4096,
            FileOptions.DeleteOnClose
        );
        await stream.WriteAsync(@event.Image, StoppingToken);

        _logger.LogQrCode(QrCode.EncodeText(@event.Url, QrCode.Ecc.Low).ToAscii(_compatibleQrCode));
        _logger.LogQrCodeLoginPrompt(_uin, @event.Url);
    }

    private void OnQrCodeQuery(BotContext lagrange, BotQrCodeQueryEvent @event)
    {
        var level = @event.State switch
        {
            TransEmpState.Confirmed or
            TransEmpState.WaitingForScan or
            TransEmpState.WaitingForConfirm => LogLevel.Debug,
            _ => LogLevel.Error,
        };
        _logger.LogQrCodeQueryState(level, @event.State);
    }

    private async Task OnCaptchaAsync(BotContext lagrange, BotCaptchaEvent @event)
    {
        var (ticket, randstr) = await _captchaResolver.ResolveCaptchaAsync(@event.CaptchaUrl, StoppingToken);
        _lagrange.SubmitCaptcha(ticket, randstr);
    }

    private async Task OnSms(BotContext lagrange, BotSMSEvent @event)
    {
        _logger.LogSmsInputPrompt();
        _lagrange.SubmitSMSCode(await Console.In.ReadLineAsync(StoppingToken) ?? string.Empty);
    }

    private void OnNewDeviceVerify(BotContext lagrange, BotNewDeviceVerifyEvent @event)
    {
        _logger.LogQrCode(QrCode.EncodeText(@event.Url, QrCode.Ecc.Low).ToAscii(_compatibleQrCode));
        _logger.LogNewDeviceVerifyPrompt(_uin, @event.Url);
    }

    public async Task StopAsync(CancellationToken ct)
    {
        _lagrange.EventInvoker.UnregisterEvent<BotRefreshKeystoreEvent>(OnRefreshKeystore);

        _cts?.Cancel();
        await _lagrange.Logout().WaitAsync(ct);
    }
}

public static partial class LagrangeLoginServiceLoggerExtension
{
    [LoggerMessage(LogLevel.Information, "\n{QrCode}")]
    public static partial void LogQrCode(this ILogger<LoginService> logger, string qrcode);

    [LoggerMessage(LogLevel.Information, "Please scan the QR code with a device logged into account {Uin} to login, URL: {Url}")]
    public static partial void LogQrCodeLoginPrompt(this ILogger<LoginService> logger, long uin, string url);

    [LoggerMessage("QR code scan state: {State}")]
    public static partial void LogQrCodeQueryState(this ILogger<LoginService> logger, LogLevel level, TransEmpState state);

    [LoggerMessage(LogLevel.Information, "Please enter the SMS verification code: ")]
    public static partial void LogSmsInputPrompt(this ILogger<LoginService> logger);

    [LoggerMessage(LogLevel.Information, "Please scan the QR code with a device logged into account {Uin} for new device verification, URL: {Url}")]
    public static partial void LogNewDeviceVerifyPrompt(this ILogger<LoginService> logger, long uin, string url);

    [LoggerMessage(LogLevel.Information, "Successfully logged in as {Uin} with protocol {Protocol}, version {Version}")]
    public static partial void LogLoginSuccess(this ILogger<LoginService> logger, long uin, Protocols protocol, string version);
}