using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;

namespace Lagrange.Core.Common.Interface.Api;

public static class OperationExt
{
    public static Task<List<BotFriend>> FetchFriends(this BotContext bot) 
        => bot.ContextCollection.Business.OperationLogic.FetchFriends();
    
    public static Task<List<BotGroupMember>> FetchMembers(this BotContext bot, uint groupUin)
        => bot.ContextCollection.Business.OperationLogic.FetchMembers(groupUin);

    public static Task<List<string>> FetchCookies(this BotContext bot, List<string> domains)
        => bot.ContextCollection.Business.OperationLogic.GetCookies(domains);
    
    public static int GetCsrfToken(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.GetCsrfToken();

    public static Task<MessageResult> SendMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.SendMessage(chain);
    
    public static Task<bool> RecallGroupMessage(this BotContext bot, uint groupUin, MessageResult result)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(groupUin, result);
}
