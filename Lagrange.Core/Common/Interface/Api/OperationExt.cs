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

    /// <summary>
    /// Fetch Notifications and requests such as friend requests and Group Join Requests
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <returns></returns>
    public static Task<List<BotGroupRequest>?> FetchRequests(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.FetchRequests();

    /// <summary>
    /// set status
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="status">The status code</param>
    /// <returns></returns>
    public static Task<bool> SetStatus(this BotContext bot, uint status)
        => bot.ContextCollection.Business.OperationLogic.SetStatus(status);

    /// <summary>
    /// set custom status
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="faceId">faceId that is same as the <see cref="Lagrange.Core.Message.Entity.FaceEntity"/></param>
    /// <param name="text">text that would shown</param>
    /// <returns></returns>
    public static Task<bool> SetCustomStatus(this BotContext bot, uint faceId, string text)
        => bot.ContextCollection.Business.OperationLogic.SetCustomStatus(faceId, text);

    public static Task<bool> GroupTransfer(this BotContext bot, uint groupUin, uint targetUin)
        => bot.ContextCollection.Business.OperationLogic.GroupTransfer(groupUin, targetUin);
    
    public static Task<bool> RequestFriend(this BotContext bot, uint targetUin, string message = "", string question = "")
        => bot.ContextCollection.Business.OperationLogic.RequestFriend(targetUin, message, question);

    public static Task<bool> Like(this BotContext bot, uint targetUin)
        => bot.ContextCollection.Business.OperationLogic.Like(targetUin);
    
    /// <summary>
    /// Get the client key for all sites
    /// </summary>
    public static Task<string?> GetClientKey(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.GetClientKey();
}
