using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Notify;
using Lagrange.OneBot.Core.Network;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Notify;

public sealed class NotifyService
{
    private readonly BotContext _bot;
    private readonly ILogger _logger;
    private readonly LagrangeWebSvcCollection _service;
    
    public NotifyService(BotContext bot, ILogger<LagrangeApp> logger, LagrangeWebSvcCollection service)
    {
        _bot = bot;
        _logger = logger;
        _service = service;

        _bot.Invoker.OnFriendRequestEvent += async (_, @event) =>
        {
            _logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotFriendRequest(_bot.BotUin, @event.SourceUin));
        };
    }
}