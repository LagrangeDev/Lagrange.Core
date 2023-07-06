using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Common.Interface.Api;

public static class OperationExt
{
    public static Task<List<BotFriend>> FetchFriends(this BotContext bot) 
        => bot.ContextCollection.Business.OperationLogic.FetchFriends();

    public static Task<List<string>> FetchCookies(this BotContext bot, List<string> domains)
        => bot.ContextCollection.Business.OperationLogic.GetCookies(domains);
}