using Lagrange.Core;
using Lagrange.OneBot.Core.Entity.Notify;
using Lagrange.OneBot.Core.Network;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Notify;

public sealed class NotifyService(BotContext bot, ILogger<NotifyService> logger, LagrangeWebSvcCollection service)
{
    public void RegisterEvents()
    {
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
            string type = @event.Type.ToString().ToLower();
            await service.SendJsonAsync(new OneBotMemberIncrease(bot.BotUin, type, @event.GroupUin, @event.InvitorUin ?? 0, @event.MemberUin));
        };
        
        bot.Invoker.OnGroupMemberDecreaseEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            string type = @event.Type.ToString().ToLower();
            await service.SendJsonAsync(new OneBotMemberDecrease(bot.BotUin, type, @event.GroupUin, @event.OperatorUin ?? 0, @event.MemberUin));
        };
        
        bot.Invoker.OnGroupMemberMuteEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            string type = @event.Duration == 0 ? "lift_ban" : "ban";
            await service.SendJsonAsync(new OneBotGroupMute(bot.BotUin, type, @event.GroupUin, @event.OperatorUin ?? 0, @event.TargetUin, @event.Duration));
        };
    }
}