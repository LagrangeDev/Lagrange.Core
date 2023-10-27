using Lagrange.OneBot.Core.Network.Service;
using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot.Core.Network;

public class LagrangeWebSvcCollection : List<ILagrangeWebService>, IHostedService
{
    public event EventHandler<MsgRecvEventArgs> OnMessageReceived = delegate {  };

    public LagrangeWebSvcCollection(IEnumerable<ILagrangeWebService> services) : base(services)
    {
        foreach (var service in this) service.OnMessageReceived += OnMessageReceived.Invoke;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {       
        foreach (var service in this) await service.StartAsync(cancellationToken);
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {       
        foreach (var service in this) await service.StopAsync(cancellationToken);
    }
    
    public async Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default)
    {
        foreach (var service in this) await service.SendJsonAsync(json, cancellationToken);
    }
}