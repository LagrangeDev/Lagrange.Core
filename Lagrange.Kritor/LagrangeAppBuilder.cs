using Lagrange.Kritor.Services;

namespace Lagrange.Kritor;

public sealed class LagrangeAppBuilder
{
    private IServiceCollection Services => _hostAppBuilder.Services;
    
    private ConfigurationManager Configuration => _hostAppBuilder.Configuration;
    
    private readonly WebApplicationBuilder _hostAppBuilder;

    internal LagrangeAppBuilder(string[] args)
    {
        _hostAppBuilder = WebApplication.CreateBuilder(args);
    }
    
    public LagrangeAppBuilder ConfigureKritorGrpcService()
    {
        Services.AddGrpc();
        
        return this;
    }
    
    public LagrangeAppBuilder ConfigureConfiguration(string path, bool optional = false, bool reloadOnChange = false)
    {
        Configuration.AddJsonFile(path, optional, reloadOnChange);
        Configuration.AddEnvironmentVariables(); // use this to configure appsettings.json with environment variables in docker container
        return this;
    }

    public LagrangeAppBuilder ConfigureKritor()
    {
        return this;
    }
    
    public LagrangeApp Build()
    {
        return new LagrangeApp(_hostAppBuilder.Build());
    }
}