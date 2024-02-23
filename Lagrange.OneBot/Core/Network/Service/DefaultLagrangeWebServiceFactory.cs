using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lagrange.OneBot.Core.Network.Service
{
    public class DefaultLagrangeWebServiceFactory(IServiceProvider services) : LagrangeWebServiceFactory(services)
    {
        public override ILagrangeWebService? Create()
        {
            var config = _config ?? throw new InvalidOperationException("Configuration must be provided");
            string? type = config["Type"] ?? (config as ConfigurationSection)?.Key;
            if (!string.IsNullOrEmpty(type))
            {
                return type switch
                {
                    "ReverseWebSocket" => Create<ReverseWSService>(config),
                    "ForwardWebSocket" => Create<ForwardWSService>(config),
                    "ReverseHttp" => Create<ReverseHttpService>(config),
                    "ForwardHttp" => Create<ForwardHttpService>(config),
                    _ => null
                };
            }

            var rws = config.GetSection("ReverseWebSocket");
            if (rws.Exists()) return Create<ReverseWSService>(rws);

            var fws = config.GetSection("ForwardWebSocket");
            if (fws.Exists()) return Create<ForwardWSService>(fws);

            var rh = config.GetSection("ReverseHttp");
            if (rh.Exists()) return Create<ReverseHttpService>(rh);

            var fh = config.GetSection("ForwardHttp");
            if (fh.Exists()) return Create<ForwardHttpService>(fh);

            return null;
        }

        protected ILagrangeWebService? Create<TService>(IConfiguration config) where TService : ILagrangeWebService
        {
            var factory = _services.GetService<ILagrangeWebServiceFactory<TService>>();
            if (factory == null) return null;

            factory.SetConfig(config);
            return factory.Create();
        }
    }
}
