using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Notify;
using Lagrange.OneBot.Core.Network;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Notify;

public sealed class NotifyService
{
    private readonly LagrangeWebSvcCollection _service;
    
    public NotifyService(BotContext bot, ILogger<LagrangeApp> logger, LagrangeWebSvcCollection service)
    {
        _service = service;

        bot.Invoker.OnFriendRequestEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotFriendRequest(bot.BotUin, @event.SourceUin));
        };

        bot.Invoker.OnGroupAdminChangedEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotGroupAdmin(bot.BotUin, @event.IsPromote ? "set" : "unset", @event.GroupUin, @event.AdminUin));
        };
        
        bot.Invoker.OnGroupMemberIncreaseEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotMemberIncrease(bot.BotUin, "invite", @event.GroupUin, @event.InvitorUin ?? 0, @event.MemberUin));
        };
        
        bot.Invoker.OnGroupMemberDecreaseEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotMemberDecrease(bot.BotUin, "leave", @event.GroupUin, @event.OperatorUin ?? 0, @event.MemberUin));
        };
    }
}