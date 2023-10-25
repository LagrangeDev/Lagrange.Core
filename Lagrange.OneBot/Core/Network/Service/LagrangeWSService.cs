using Lagrange.OneBot.Core.Entity.Meta;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Network.Service;

public abstract class LagrangeWSService(IConfiguration config, ILogger<LagrangeApp> logger) : ILagrangeWebService
{
    protected readonly ILogger Logger = logger;

    protected readonly IConfiguration Config = config;
    
    protected void OnHeartbeat(object? sender)
    {
        var status = new OneBotStatus(true, true);
        var heartBeat = new OneBotHeartBeat(
            Config.GetValue<uint>("Account:Uin"), 
            Config.GetValue<int>("Implementation:ReverseWebSocket:HeartBeatInterval"), 
            status);
        
        SendJsonAsync(heartBeat);
    }

    public abstract Task StartAsync(CancellationToken cancellationToken);

    public abstract Task StopAsync(CancellationToken cancellationToken);

    public abstract event EventHandler<MsgRecvEventArgs>? OnMessageReceived;
    public abstract Task SendJsonAsync<T>(T json, CancellationToken cancellationToken = default);
}