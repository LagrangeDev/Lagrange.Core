using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lagrange.OneBot.Core.Network.Service
{
    public class DefaultLagrangeWebServiceFactory : LagrangeWebServiceFactory
    {
        public DefaultLagrangeWebServiceFactory(IServiceProvider services) : base(services)
        {
            
        }

        public override ILagrangeWebService? Create()
        {
            var config = _config ?? throw new InvalidOperationException("Configuration must be provided");
            var type = config["Type"];
            if (!string.IsNullOrEmpty(type))
            {
                return type switch
                {
                    "ReverseWebSocket" => Create<ReverseWSService>(config),
                    "ForwardWebSocket" => Create<ForwardWSService>(config),
                    _ => null
                };
            }
            var rws = config.GetSection("ReverseWebSocket");
            if (rws.Exists())
            {
                return Create<ReverseWSService>(rws);
            }
            var fws = config.GetSection("ForwardWebSocket");
            if (fws.Exists())
            {
                return Create<ReverseWSService>(fws);
            }
            return null;
        }

        protected ILagrangeWebService? Create<TService>(IConfiguration config) where TService : ILagrangeWebService
        {
            var factory = _services.GetService<ILagrangeWebServiceFactory<TService>>();
            if (factory == null)
            {
                return null;
            }
            factory.SetConfig(config);
            return factory.Create();
        }
    }
}
