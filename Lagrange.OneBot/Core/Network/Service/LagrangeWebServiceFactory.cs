using Microsoft.Extensions.Configuration;

namespace Lagrange.OneBot.Core.Network.Service
{
    public interface ILagrangeWebServiceFactory
    {
        void SetConfig(IConfiguration config);

        ILagrangeWebService? Create();
    }

    public interface ILagrangeWebServiceFactory<TService> : ILagrangeWebServiceFactory where TService : ILagrangeWebService
    {

    }

    public abstract class LagrangeWebServiceFactory : ILagrangeWebServiceFactory
    {
        protected readonly IServiceProvider _services;

        protected IConfiguration? _config;

        protected LagrangeWebServiceFactory(IServiceProvider services)
        {
            _services = services;
        }

        public void SetConfig(IConfiguration config)
        {
            _config = config;
        }

        public abstract ILagrangeWebService? Create();
    }
}
