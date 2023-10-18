using System.Text.Json;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.OneBot.Core.Message;
using Lagrange.OneBot.Core.Network;
using Lagrange.OneBot.Core.Operation;
using Lagrange.OneBot.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LogLevel = Lagrange.Core.Internal.Event.EventArg.LogLevel;

namespace Lagrange.OneBot;

public class LagrangeApp : IHost
{
    private readonly IHost _hostApp;
    
    public IServiceProvider Services => _hostApp.Services;

    public ILogger<LagrangeApp> Logger { get; }
    
    public IConfiguration Configuration => Services.GetRequiredService<IConfiguration>();
    
    public BotContext Instance => Services.GetRequiredService<BotContext>();
    
    public ILagrangeWebService WebService => Services.GetRequiredService<ILagrangeWebService>();

    public MessageService MessageService { get; set; }
    
    public OperationService OperationService { get; set; }
    
    internal LagrangeApp(IHost host)
    {
        _hostApp = host;
        Logger = Services.GetRequiredService<ILogger<LagrangeApp>>();

        MessageService = Services.GetRequiredService<MessageService>();
        OperationService = Services.GetRequiredService<OperationService>();
    }

    public async Task StartAsync(CancellationToken cancellationToken = new())
    {
        await _hostApp.StartAsync(cancellationToken);
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

        Instance.Invoker.OnBotOnlineEvent += async (_, args) =>
        {
            var keystore = Instance.UpdateKeystore();
            Logger.LogInformation($"Bot Online: {keystore.Uin}");
            string json = JsonSerializer.Serialize(keystore, new JsonSerializerOptions { WriteIndented = true });
            
            await File.WriteAllTextAsync(Configuration["ConfigPath:Keystore"] ?? "keystore.json", json, cancellationToken);
        };
        
        await WebService.StartAsync(cancellationToken);
        
        if (Configuration["Account:Password"] == null || Configuration["Account:Password"] == "" || 
            Instance.ContextCollection.Keystore.Session.TempPassword == null)
        {
            Logger.LogInformation("Session expired or Password not filled in, try to login by QrCode");
            
            var qrCode = await Instance.FetchQrCode();
            if (qrCode != null)
            {
                QrCodeHelper.Output(qrCode.Value.Url ?? "");
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
    }

    public async Task StopAsync(CancellationToken cancellationToken = new())
    {
        Logger.LogInformation("Lagrange.OneBot Implementation has stopped");
        
        Instance.Dispose();
        
        await WebService.StopAsync(cancellationToken);
        await _hostApp.StopAsync(cancellationToken);
    }
    
    public void Dispose()
    {
        _hostApp.Dispose();
        GC.SuppressFinalize(this);
    }
}