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

    public abstract class LagrangeWebServiceFactory(IServiceProvider services) : ILagrangeWebServiceFactory
    {
        protected readonly IServiceProvider _services = services;

        protected IConfiguration? _config;

        public void SetConfig(IConfiguration config) => _config = config;

        public abstract ILagrangeWebService? Create();
    }
}
