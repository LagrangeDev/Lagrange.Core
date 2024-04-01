namespace Lagrange.Kritor;

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
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}