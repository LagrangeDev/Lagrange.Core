using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot.Core.Network;

public interface ILagrangeWebService : IHostedService
{
    public event EventHandler<MsgRecvEventArgs> OnMessageReceived;
        
    public Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default);
}