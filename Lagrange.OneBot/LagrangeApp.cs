using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot;

public class LagrangeApp : IHost
{
    private readonly IHost _hostApp;
    
    public IServiceProvider Services => _hostApp.Services;

    public ILogger<LagrangeApp> Logger { get; }
    
    internal LagrangeApp(IHost host)
    {
        _hostApp = host;
        Logger = Services.GetRequiredService<ILogger<LagrangeApp>>();
    }

    public async Task StartAsync(CancellationToken cancellationToken = new())
    {
        Logger.LogInformation("Lagrange.OneBot Implementation has started");
        await _hostApp.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = new())
    {
        Logger.LogInformation("Lagrange.OneBot Implementation has stopped");
        await _hostApp.StopAsync(cancellationToken);
    }
    
    public void Dispose()
    {
        _hostApp.Dispose();
        GC.SuppressFinalize(this);
    }
}