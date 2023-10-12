using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;

namespace Lagrange.Core.Common.Interface.Api;

public static class OperationExt
{
    public static Task<List<BotFriend>> FetchFriends(this BotContext bot, bool refreshCache = false) 
        => bot.ContextCollection.Business.OperationLogic.FetchFriends(refreshCache);
    
    public static Task<List<BotGroupMember>> FetchMembers(this BotContext bot, uint groupUin, bool refreshCache = false)
        => bot.ContextCollection.Business.OperationLogic.FetchMembers(groupUin, refreshCache);
    
    public static Task<List<BotGroup>> FetchGroups(this BotContext bot, bool refreshCache = false)
        => bot.ContextCollection.Business.OperationLogic.FetchGroups(refreshCache);

    public static Task<List<string>> FetchCookies(this BotContext bot, List<string> domains)
        => bot.ContextCollection.Business.OperationLogic.GetCookies(domains);

    public static Task<MessageResult> SendMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.SendMessage(chain);
    
    public static Task<bool> RecallGroupMessage(this BotContext bot, uint groupUin, MessageResult result)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(groupUin, result);
    
    public static Task<bool> RecallGroupMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(chain);
    
    public static Task<bool> RequestFriend(this BotContext bot, uint targetUin, string message = "", string question = "")
        => bot.ContextCollection.Business.OperationLogic.RequestFriend(targetUin, message, question);
    
    public static Task<string?> GetClientKey(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.GetClientKey();
}
