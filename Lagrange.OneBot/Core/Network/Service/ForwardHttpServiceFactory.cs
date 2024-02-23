using Lagrange.OneBot.Core.Network.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Lagrange.OneBot.Core.Network.Service
{
    public sealed class ForwardHttpServiceFactory(IServiceProvider services) : LagrangeWebServiceFactory(services), ILagrangeWebServiceFactory<ForwardHttpService>
    {
        public override ILagrangeWebService Create()
        {
            var config = _config ?? throw new InvalidOperationException("Configuration must be provided");
            var options = _services.GetRequiredService<IOptionsSnapshot<ForwardHttpServiceOptions>>();
            config.Bind(options.Value);

            return _services.GetRequiredService<ForwardHttpService>();
        }
    }
}
