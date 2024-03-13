using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;
using Lagrange.OneBot.Core.Entity.Notify;
using Lagrange.OneBot.Core.Network;
using Lagrange.OneBot.Database;
using Microsoft.Extensions.Logging;

namespace Lagrange.OneBot.Core.Notify;

public sealed class NotifyService(BotContext bot, ILogger<NotifyService> logger, LagrangeWebSvcCollection service)
{
    public void RegisterEvents()
    {
        bot.Invoker.OnGroupMessageReceived += async (_, @event) =>
        {
            if (@event.Chain.GetEntity<FileEntity>() is { FileId: { } id } file)
            {
                var fileInfo = new OneBotFileInfo(id, file.FileName, (ulong)file.FileSize, file.FileUrl ?? "");
                await service.SendJsonAsync(new OneBotGroupFile(bot.BotUin, @event.Chain.GroupUin ?? 0, @event.Chain.FriendUin, fileInfo));
            }
        };

        bot.Invoker.OnGroupMuteEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotGroupMute(bot.BotUin, @event.IsMuted ? "ban" : "lift_ban", @event.GroupUin, @event.OperatorUin ?? 0, 0, @event.IsMuted ? -1 : 0));
        };
        
        bot.Invoker.OnFriendRequestEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotFriendRequestNotice(bot.BotUin, @event.SourceUin));
            await service.SendJsonAsync(new OneBotFriendRequest(bot.BotUin, @event.SourceUin, @event.Message, @event.SourceUid));
        };

        bot.Invoker.OnGroupInvitationReceived += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());

            var requests = await bot.FetchGroupRequests();
            if (requests?.FirstOrDefault(x => @event.GroupUin == x.GroupUin && @event.InvitorUin == x.InvitorMemberUin) is { } request)
            {
                string flag = $"{request.Sequence}-{request.GroupUin}-{(uint)request.EventType}";
                await service.SendJsonAsync(new OneBotGroupRequest(bot.BotUin, @event.InvitorUin, @event.GroupUin, "invite", request.Comment, flag));
            }
        };
        
        bot.Invoker.OnGroupJoinRequestEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());

            var requests = await bot.FetchGroupRequests();
            if (requests?.FirstOrDefault(x => @event.GroupUin == x.GroupUin && @event.TargetUin == x.TargetMemberUin) is { } request)
            {
                string flag = $"{request.Sequence}-{request.GroupUin}-{(uint)request.EventType}";
                await service.SendJsonAsync(new OneBotGroupRequest(bot.BotUin, @event.TargetUin, @event.GroupUin, "add", request.Comment, flag));
            }
        };
        
        bot.Invoker.OnGroupInvitationRequestEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());

            var requests = await bot.FetchGroupRequests();
            if (requests?.FirstOrDefault(x => @event.GroupUin == x.GroupUin && @event.TargetUin == x.TargetMemberUin) is { } request)
            {
                string flag = $"{request.Sequence}-{request.GroupUin}-{(uint)request.EventType}";
                await service.SendJsonAsync(new OneBotGroupRequest(bot.BotUin, @event.TargetUin, @event.GroupUin, "add", request.Comment, flag) { InvitorId = @event.InvitorUin });
            }
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

        bot.Invoker.OnGroupRecallEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotGroupRecall(bot.BotUin)
            {
                GroupId = @event.GroupUin,
                UserId = @event.AuthorUin,
                MessageId = MessageRecord.CalcMessageHash(@event.Random, @event.Sequence),
                OperatorId = @event.OperatorUin
            });
        };
        
        bot.Invoker.OnFriendRecallEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotFriendRecall(bot.BotUin)
            {
                UserId = @event.FriendUin,
                MessageId = MessageRecord.CalcMessageHash(@event.Random, @event.Sequence),
            });
        };

        bot.Invoker.OnFriendPokeEvent += async (_, @event) =>
        {
            logger.LogInformation(@event.ToString());
            await service.SendJsonAsync(new OneBotFriendPoke(bot.BotUin)
            {
                SenderId = @event.FriendUin,
                UserId = @event.FriendUin,
                TargetId = bot.BotUin
            });
        };
    }
}
