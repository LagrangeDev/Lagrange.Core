using Lagrange.OneBot.Core.Network.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Network;

public class LagrangeWebSvcCollection : Dictionary<string, ILagrangeWebService>, IHostedService
{
    private const string Tag = nameof(LagrangeWebSvcCollection);
    
    public event EventHandler<MsgRecvEventArgs> OnMessageReceived = delegate {  };

    public LagrangeWebSvcCollection(IConfiguration global, ILogger<LagrangeApp> logger)
    {
        uint uin = global.GetValue<uint>("Account:Uin");
        
        if (global.GetSection("Implementations").Exists())
        {
            logger.LogInformation($"[{Tag}]: Multi Connection has been configured");

            foreach (var section in global.GetSection("Implementations").GetChildren())
            {
                ILagrangeWebService? service = section["Type"] switch
                {
                    "ReverseWebSocket" => new ReverseWSService(section, logger, uin),
                    "ForwardWebSocket" => new ForwardWSService(section, logger, uin),
                    _ => null
                };

                if (service == null) logger.LogWarning($"[{Tag}]: unknown type of service of {section["Type"]} is configured, skipped");
                else Add(new Guid().ToString(), service);
            }
        }
        else if (global.GetSection("Implementation").Exists())
        {
            logger.LogInformation($"[{Tag}]: Single Connection has been configured");

            string identifier = new Guid().ToString();
            if (global.GetSection("Implementation:ReverseWebSocket").Exists())
            {
                this[identifier] = new ReverseWSService(global.GetSection("Implementation:ReverseWebSocket"), logger, uin);
            }
            else if (global.GetSection("Implementation:ForwardWebSocket").Exists())
            {
                this[identifier] = new ForwardWSService(global.GetSection("Implementation:ForwardWebSocket"), logger, uin);
            }
        }
        else
        {
            logger.LogWarning($"[{Tag}]: No implementation has been configured");
        }

        foreach (var (identifier, service) in this)
        {
            service.OnMessageReceived += (sender, args) =>
            {
                OnMessageReceived.Invoke(sender, new MsgRecvEventArgs(identifier, args.Data));
            };
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {       
        foreach (var (_, service) in this) await service.StartAsync(cancellationToken);
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {       
        foreach (var (_, service) in this) await service.StopAsync(cancellationToken);
    }
    
    public async Task SendJsonAsync<T>(T json, string? identifier = null, CancellationToken cancellationToken = default)
    {
        if (identifier == null)
        {
            foreach (var (_, service) in this) await service.SendJsonAsync(json, cancellationToken);
        }
        else
        {
            if (TryGetValue(identifier, out var service)) await service.SendJsonAsync(json, cancellationToken);
        }
    }
}