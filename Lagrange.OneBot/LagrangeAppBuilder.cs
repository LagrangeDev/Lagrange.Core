using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot;

public sealed class LagrangeAppBuilder
{
    private IServiceCollection Services => _hostAppBuilder.Services;
    
    private ConfigurationManager Configuration => _hostAppBuilder.Configuration;
    
    private readonly HostApplicationBuilder _hostAppBuilder;

    internal LagrangeAppBuilder(string[] args)
    {
        _hostAppBuilder = new HostApplicationBuilder(args);
    }
    
    public LagrangeAppBuilder ConfigureConfiguration(string path, bool optional = false, bool reloadOnChange = false)
    {
        Configuration.AddJsonFile(path, optional, reloadOnChange);
        return this;
    }

    public LagrangeApp Build()
    {
        var app = new LagrangeApp(_hostAppBuilder.Build());
        return app;
    }
}