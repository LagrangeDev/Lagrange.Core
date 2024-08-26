using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot.Core.Network.Service;

public interface ILagrangeWebService : IHostedService
{
    public event EventHandler<MsgRecvEventArgs> OnMessageReceived;

    public ValueTask SendJsonAsync<T>(T json, string? identifier = null, CancellationToken cancellationToken = default);
}
