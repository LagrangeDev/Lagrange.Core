using System.Threading;
using System.Threading.Tasks;
using Lagrange.Core;
using Lagrange.Core.Events.EventArgs;
using Microsoft.Extensions.Hosting;

namespace Lagrange.Milky.Caching;

public class CacheService(BotContext lagrange, MessageCache cache) : IHostedService
{
    private readonly BotContext _lagrange = lagrange;
    private readonly MessageCache _cache = cache;

    public Task StartAsync(CancellationToken ct)
    {
        _lagrange.EventInvoker.RegisterEvent<BotMessageEvent>(OnMessage);
        return Task.CompletedTask;
    }

    private void OnMessage(BotContext lagrange, BotMessageEvent @event)
    {
        _cache.Set(@event.Message);
    }

    public Task StopAsync(CancellationToken ct)
    {
        _lagrange.EventInvoker.UnregisterEvent<BotMessageEvent>(OnMessage);
        return Task.CompletedTask;
    }
}