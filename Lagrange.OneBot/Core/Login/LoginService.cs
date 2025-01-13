using System.Text.Json;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Lagrange.OneBot.Core.Network;
using Lagrange.OneBot.Core.Notify;
using Lagrange.OneBot.Utility;
using Lagrange.OneBot.Utility.Fallbacks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using LagrangeLogLevel = Lagrange.Core.Event.EventArg.LogLevel;
using MicrosoftLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Lagrange.OneBot.Core.Login;

public class LoginService(IConfiguration configuration, ILogger<LoginService> logger, ILogger<BotContext> botLogger, BotContext lagrange, LagrangeWebSvcCollection web, NotifyService notify) : IHostedService
{
    private readonly ILogger<LoginService> _logger = logger;

    private readonly ILogger<BotContext> _botLogger = botLogger;

    private readonly BotContext _lagrange = lagrange;

    private readonly LagrangeWebSvcCollection _web = web;

    private readonly NotifyService _notify = notify;

    private readonly bool _isCompatibility = configuration.GetValue<bool>("QrCode:ConsoleCompatibilityMode");

    public async Task StartAsync(CancellationToken token)
    {
        _logger.LogInformation("Protocol Version: {}", _lagrange.AppInfo.CurrentVersion);

        _lagrange.Invoker.OnBotLogEvent += BotLogHandler;
        _notify.RegisterEvents();

        bool isSucceed = await FallbackAsync.Create()
            .Add((token) =>
            {
                var keystore = _lagrange.UpdateKeystore();
                if (keystore.Session.TempPassword == null) return Task.FromResult(false);
                if (keystore.Session.TempPassword.Length == 0) return Task.FromResult(false);
                return _lagrange.LoginByEasy(token);
            })
            .Add(async (token) =>
            {
                (string Url, byte[] QrCode)? qrcode = await _lagrange.FetchQrCode().WaitAsync(token);
                if (!qrcode.HasValue) return false;

                await File.WriteAllBytesAsync($"qr-{configuration["Account:Uin"]}.png", qrcode.Value.QrCode, token);
                QrCodeHelper.Output(qrcode.Value.Url, _isCompatibility);

                return await (Task<bool>)_lagrange.LoginByQrCode(token);
            })
            .ExecuteAsync(token);

        if (!isSucceed) throw new Exception("All login failed!");

        string keystoreJson = JsonSerializer.Serialize(_lagrange.UpdateKeystore());
        File.WriteAllText(configuration["ConfigPath:Keystore"] ?? "keystore.json", keystoreJson);

        _logger.LogInformation("Bot Uin: {}", _lagrange.BotUin);

        await _web.StartAsync(token);
    }

    private void BotLogHandler(BotContext context, BotLogEvent e)
    {
        _botLogger.Log(e.Level switch
        {
            LagrangeLogLevel.Debug => MicrosoftLogLevel.Trace,
            LagrangeLogLevel.Verbose => MicrosoftLogLevel.Information,
            LagrangeLogLevel.Information => MicrosoftLogLevel.Information,
            LagrangeLogLevel.Warning => MicrosoftLogLevel.Warning,
            LagrangeLogLevel.Fatal => MicrosoftLogLevel.Error,
            _ => MicrosoftLogLevel.Error
        }, "{}", e.ToString());
    }

    public Task StopAsync(CancellationToken token)
    {
        _lagrange.Invoker.OnBotLogEvent -= BotLogHandler;
        _lagrange.Dispose();

        return Task.CompletedTask;
    }
}