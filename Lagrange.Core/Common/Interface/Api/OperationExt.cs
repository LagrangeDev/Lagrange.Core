using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;

namespace Lagrange.Core.Common.Interface.Api;

public static class OperationExt
{
    public static Task<List<BotFriend>> FetchFriends(this BotContext bot) 
        => bot.ContextCollection.Business.OperationLogic.FetchFriends();

    public static Task<List<string>> FetchCookies(this BotContext bot, List<string> domains)
        => bot.ContextCollection.Business.OperationLogic.GetCookies(domains);
    
    public static int GetCsrfToken(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.GetCsrfToken();

    public static Task<bool> SendMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.SendMessage(chain);
}