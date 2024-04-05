using Lagrange.Kritor.Services;

namespace Lagrange.Kritor;

public class LagrangeApp : IHost
{
    private readonly WebApplication _hostApp;

    public IServiceProvider Services => _hostApp.Services;
    
    public ILogger<LagrangeApp> Logger { get; }
        
    internal LagrangeApp(WebApplication host)
    {
        _hostApp = host;
        Logger = Services.GetRequiredService<ILogger<LagrangeApp>>();
    }
    
    public void Dispose()
    {
        
    }

    public async Task StartAsync(CancellationToken cancellationToken = new())
    {
        _hostApp.MapGrpcService<AuthService>();
        _hostApp.MapGrpcService<CoreService>();
        _hostApp.MapGrpcService<CustomizationService>();
        _hostApp.MapGrpcService<EventService>();
        _hostApp.MapGrpcService<FriendService>();
        _hostApp.MapGrpcService<GroupFileService>();
        _hostApp.MapGrpcService<GroupService>();
        _hostApp.MapGrpcService<MessageService>();
        _hostApp.MapGrpcService<ReverseService>();
        _hostApp.MapGrpcService<WebService>();
        
        _hostApp.Start();
    }

    public async Task StopAsync(CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }
}