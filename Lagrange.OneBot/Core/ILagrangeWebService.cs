using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot.Core;

public interface ILagrangeWebService : IHostedService
{
    public Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default);
}