using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

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
    public static Task<List<BotGroupRequest>?> FetchGroupRequests(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupRequests();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    public static Task<List<dynamic>?> FetchFriendRequests(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.FetchFriendRequests();

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
    
    public static Task<bool> RequestFriend(this BotContext bot, uint targetUin, string question = "", string message = "")
        => bot.ContextCollection.Business.OperationLogic.RequestFriend(targetUin, question, message);

    public static Task<bool> Like(this BotContext bot, uint targetUin, uint count = 1)
        => bot.ContextCollection.Business.OperationLogic.Like(targetUin, count);
    
    /// <summary>
    /// Get the client key for all sites
    /// </summary>
    public static Task<string?> GetClientKey(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.GetClientKey();

    /// <summary>
    /// Get the history message record, max 30 seqs
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">target GroupUin</param>
    /// <param name="startSequence">Start Sequence of the message</param>
    /// <param name="endSequence">End Sequence of the message</param>
    public static Task<List<MessageChain>?> GetGroupMessage(this BotContext bot, uint groupUin, uint startSequence, uint endSequence)
        => bot.ContextCollection.Business.OperationLogic.GetGroupMessage(groupUin, startSequence, endSequence);
    
    /// <summary>
    /// Get the history message record for private message
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="friendUin">target FriendUin</param>
    /// <param name="timestamp">timestamp of the message chain</param>
    /// <param name="count">number of message to be fetched before timestamp</param>
    public static Task<List<MessageChain>?> GetRoamMessage(this BotContext bot, uint friendUin, uint timestamp, uint count)
        => bot.ContextCollection.Business.OperationLogic.GetRoamMessage(friendUin, timestamp, count);
    
    /// <summary>
    /// Get the history message record for private message
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="targetChain">target chain</param>
    /// <param name="count">number of message to be fetched before timestamp</param>
    public static Task<List<MessageChain>?> GetRoamMessage(this BotContext bot, MessageChain targetChain, uint count)
    {
        uint timestamp = (uint)(targetChain.Time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        return bot.ContextCollection.Business.OperationLogic.GetRoamMessage(targetChain.FriendUin, timestamp, count);
    }
    
    public static Task<BotUserInfo?> FetchUserInfo(this BotContext bot, uint uin)
        => bot.ContextCollection.Business.OperationLogic.FetchUserInfo(uin);
    
    public static Task<List<string>?> FetchCustomFace(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.FetchCustomFace();
    
    public static Task<string?> UploadLongMessage(this BotContext bot, List<MessageChain> chains)
        => bot.ContextCollection.Business.OperationLogic.UploadLongMessage(chains);
    
    public static Task<bool> MarkAsRead(this BotContext bot, MessageChain targetChain)
    {
        uint timestamp = (uint)(targetChain.Time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        return bot.ContextCollection.Business.OperationLogic.MarkAsRead(targetChain.GroupUin ?? 0, targetChain.Uid, targetChain.Sequence, timestamp);
    }
    
    public static Task<bool> UploadFriendFile(this BotContext bot, uint targetUin, FileEntity fileEntity) 
        => bot.ContextCollection.Business.OperationLogic.UploadFriendFile(targetUin, fileEntity);
    
    public static Task<bool> FriendPoke(this BotContext bot, uint friendUin)
        => bot.ContextCollection.Business.OperationLogic.FriendPoke(friendUin);

}
