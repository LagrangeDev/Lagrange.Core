using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LogLevel = Lagrange.Core.Core.Event.EventArg.LogLevel;

namespace Lagrange.OneBot;

public class LagrangeApp : IHost
{
    private readonly IHost _hostApp;
    
    public IServiceProvider Services => _hostApp.Services;

    public ILogger<LagrangeApp> Logger { get; }
    
    public IConfiguration Configuration => Services.GetRequiredService<IConfiguration>();
    
    public BotContext Instance => Services.GetRequiredService<BotContext>();
    
    internal LagrangeApp(IHost host)
    {
        _hostApp = host;
        Logger = Services.GetRequiredService<ILogger<LagrangeApp>>();
    }

    public async Task StartAsync(CancellationToken cancellationToken = new())
    {
        Logger.LogInformation("Lagrange.OneBot Implementation has started");
        Logger.LogInformation($"Protocol: {Configuration.GetValue<string>("Protocol")} | {Instance.ContextCollection.AppInfo.CurrentVersion}");
        
        Instance.Invoker.OnBotLogEvent += (_, args) => Logger.Log(args.Level switch
        {
            LogLevel.Debug => Microsoft.Extensions.Logging.LogLevel.Trace,
            LogLevel.Verbose => Microsoft.Extensions.Logging.LogLevel.Information,
            LogLevel.Information => Microsoft.Extensions.Logging.LogLevel.Information,
            LogLevel.Warning => Microsoft.Extensions.Logging.LogLevel.Warning,
            LogLevel.Fatal => Microsoft.Extensions.Logging.LogLevel.Error,
            _ => Microsoft.Extensions.Logging.LogLevel.Error
        }, args.ToString());

        if (Configuration.GetValue<uint>("Account:Uin") == 0)
        {
            var qrCode = await Instance.FetchQrCode();
            if (qrCode != null)
            {
                QrCodeHelper.Output(Instance.ContextCollection.Keystore.Session.QrUrl ?? "");
                await Instance.LoginByQrCode();
            }
        }
        else
        {
            Instance.Invoker.OnBotCaptchaEvent += (_, args) =>
            {
                Logger.LogWarning($"Captcha: {args.Url}");
                Logger.LogWarning("Please input ticket and randomString:");
                
                var ticket = Console.ReadLine();
                var randomString = Console.ReadLine();

                if (ticket != null && randomString != null) Instance.SubmitCaptcha(ticket, randomString);
            };
            
            await Instance.LoginByPassword();
        }
        
        await _hostApp.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = new())
    {
        Logger.LogInformation("Lagrange.OneBot Implementation has stopped");
        
        Instance.Dispose();
        
        await _hostApp.StopAsync(cancellationToken);
    }
    
    public void Dispose()
    {
        _hostApp.Dispose();
        GC.SuppressFinalize(this);
    }
}