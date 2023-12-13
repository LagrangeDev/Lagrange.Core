using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;

namespace Lagrange.Core.Common.Interface.Api;

public static class OperationExt
{
    /// <summary>
    /// Fetch the friend list of account from server or cache
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="refreshCache">force the cache to be refreshed</param>
    /// <returns></returns>
    public static Task<List<BotFriend>> FetchFriends(this BotContext bot, bool refreshCache = false) 
        => bot.ContextCollection.Business.OperationLogic.FetchFriends(refreshCache);
    
    /// <summary>
    /// Fetch the member list of the group from server or cache
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin"></param>
    /// <param name="refreshCache">force the cache to be refreshed</param>
    /// <returns></returns>
    public static Task<List<BotGroupMember>> FetchMembers(this BotContext bot, uint groupUin, bool refreshCache = false)
        => bot.ContextCollection.Business.OperationLogic.FetchMembers(groupUin, refreshCache);
    
    /// <summary>
    /// Fetch the target Stranger from cache <br/>
    /// (The implementation of this api relies on the group_member_info, so it can only get information from the cache; If refreshCache=1, an empty body is returned)
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="refreshCache">force the cache to be refreshed</param>
    /// <returns></returns>
    public static Task<BotStranger> FetchStranger(this BotContext bot, uint targetUin , bool refreshCache = false)
        => bot.ContextCollection.Business.OperationLogic.FetchStranger(targetUin,refreshCache);
    
    
    
    /// <summary>
    /// Fetch the group list of the account from server or cache
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="refreshCache">force the cache to be refreshed</param>
    /// <returns></returns>
    public static Task<List<BotGroup>> FetchGroups(this BotContext bot, bool refreshCache = false)
        => bot.ContextCollection.Business.OperationLogic.FetchGroups(refreshCache);

    /// <summary>
    /// Fetch the cookies/pskey for accessing other site
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="domains">the domain for the cookie to be valid</param>
    /// <returns>the list of cookies</returns>
    public static Task<List<string>> FetchCookies(this BotContext bot, List<string> domains)
        => bot.ContextCollection.Business.OperationLogic.GetCookies(domains);

    /// <summary>
    /// Send the message
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="chain">the chain constructed by <see cref="MessageBuilder"/></param>
    public static Task<MessageResult> SendMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.SendMessage(chain);
    
    /// <summary>
    /// Recall the group message from Bot itself by <see cref="MessageResult"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group of the message</param>
    /// <param name="result">The return value for <see cref="SendMessage"/></param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallGroupMessage(this BotContext bot, uint groupUin, MessageResult result)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(groupUin, result);
    
    /// <summary>
    /// Recall the group message by <see cref="MessageChain"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="chain">target MessageChain, must be Group</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallGroupMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(chain);
    
    public static Task<bool> RequestFriend(this BotContext bot, uint targetUin, string message = "", string question = "")
        => bot.ContextCollection.Business.OperationLogic.RequestFriend(targetUin, message, question);
    
    /// <summary>
    /// Get the client key for all sites
    /// </summary>
    public static Task<string?> GetClientKey(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.GetClientKey();
}
