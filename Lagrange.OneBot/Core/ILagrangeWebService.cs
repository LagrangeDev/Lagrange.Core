using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot.Core;

public interface ILagrangeWebService : IHostedService
{
    public ValueTask SendJsonAsync<T>(T json, CancellationToken cancellationToken = default);
}