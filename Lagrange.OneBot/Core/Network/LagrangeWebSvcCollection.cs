using Lagrange.OneBot.Core.Network.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Network;

public sealed partial class LagrangeWebSvcCollection(IServiceProvider services, IConfiguration config, ILogger<LagrangeApp> logger) 
    : IHostedService
{
    private const string Tag = nameof(LagrangeWebSvcCollection);
    
    public event EventHandler<MsgRecvEventArgs>? OnMessageReceived;

    private readonly List<(IServiceScope, ILagrangeWebService)> _webServices = new();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var implsSection = config.GetSection("Implementations");
        if (implsSection.Exists())
        {
            Log.LogMultiConnection(logger, Tag);
        }
        else 
        {
            implsSection = config.GetSection("Implementation");
            if (!implsSection.Exists())
            {
                Log.LogNoConnection(logger, Tag);
                return;
            }
            
            Log.LogSingleConnection(logger, Tag);
        }

        foreach (var section in implsSection.GetChildren())
        {
            var scope = services.CreateScope();
            var factory = services.GetRequiredService<ILagrangeWebServiceFactory>();
            factory.SetConfig(section);
            
            var webService = services.GetRequiredService<ILagrangeWebService>();
            webService.OnMessageReceived += (sender, args) =>
            {
                OnMessageReceived?.Invoke(sender, new MsgRecvEventArgs(args.Data));
            };
            
            try
            {
                await webService.StartAsync(cancellationToken);
                _webServices.Add((scope, webService));
            }
            catch (Exception e)
            {
                Log.LogWebServiceStartFailed(logger, e, Tag);
                scope.Dispose();
            }
        }
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {       
        foreach (var (scope, service) in _webServices)
        {
            try
            {
                await service.StopAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Log.LogWebServiceStopFailed(logger, e, Tag);
            }
            finally
            {
                scope.Dispose();
            }
        }
    }
    
    public async Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        foreach (var (_, service) in _webServices)
        {
            try
            {
                var vt = service.SendJsonAsync(json, cancellationToken);
                if (!vt.IsCompletedSuccessfully)
                {
                    var t = vt.AsTask();
                    await t.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
            catch (Exception e)
            {
                Log.LogWebServiceSendFailed(logger, e, Tag);
            }
        }
    }

    private static partial class Log
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "[{tag}]: Multi Connection has been configured")]
        public static partial void LogMultiConnection(ILogger logger, string tag);

        [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "[{tag}]: Single Connection has been configured")]
        public static partial void LogSingleConnection(ILogger logger, string tag);

        [LoggerMessage(EventId = 3, Level = LogLevel.Warning, Message = "[{Tag}]: No implementation has been configured")]
        public static partial void LogNoConnection(ILogger logger, string tag);

        [LoggerMessage(EventId = 4, Level = LogLevel.Warning, Message = "[{Tag}]: WebService start failed.")]
        public static partial void LogWebServiceStartFailed(ILogger logger, Exception e, string tag);

        [LoggerMessage(EventId = 5, Level = LogLevel.Warning, Message = "[{Tag}]: WebService stop failed.")]
        public static partial void LogWebServiceStopFailed(ILogger logger, Exception e, string tag);

        [LoggerMessage(EventId = 6, Level = LogLevel.Warning, Message = "[{Tag}]: WebService send message failed.")]
        public static partial void LogWebServiceSendFailed(ILogger logger, Exception e, string tag);
    }
}