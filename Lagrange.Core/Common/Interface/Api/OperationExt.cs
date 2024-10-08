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
        => bot.ContextCollection.Business.OperationLogic.FetchFriends(CancellationToken.None, refreshCache);


    /// <summary>
    /// Fetch the friend list of account from server or cache
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="refreshCache">force the cache to be refreshed</param>
    /// <returns></returns>
    public static Task<List<BotFriend>> FetchFriends(this BotContext bot, bool refreshCache, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FetchFriends(cancellationToken, refreshCache);

    /// <summary>
    /// Fetch the member list of the group from server or cache
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin"></param>
    /// <param name="refreshCache">force the cache to be refreshed</param>
    /// <returns></returns>
    public static Task<List<BotGroupMember>> FetchMembers(this BotContext bot, uint groupUin, bool refreshCache = false)
        => bot.ContextCollection.Business.OperationLogic.FetchMembers(groupUin, CancellationToken.None, refreshCache);

    /// <summary>
    /// Fetch the member list of the group from server or cache
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin"></param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="refreshCache">force the cache to be refreshed</param>
    /// <returns></returns>
    public static Task<List<BotGroupMember>> FetchMembers(this BotContext bot, uint groupUin, bool refreshCache, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FetchMembers(groupUin, cancellationToken, refreshCache);

    /// <summary>
    /// Fetch the group list of the account from server or cache
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="refreshCache">force the cache to be refreshed</param>
    /// <returns></returns>
    public static Task<List<BotGroup>> FetchGroups(this BotContext bot, bool refreshCache = false)
        => bot.ContextCollection.Business.OperationLogic.FetchGroups(refreshCache, CancellationToken.None);

    /// <summary>
    /// Fetch the group list of the account from server or cache
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <param name="refreshCache">force the cache to be refreshed</param>
    /// <returns></returns>
    public static Task<List<BotGroup>> FetchGroups(this BotContext bot, bool refreshCache, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FetchGroups(refreshCache, cancellationToken);

    /// <summary>
    /// Fetch the cookies/pskey for accessing other site
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="domains">the domain for the cookie to be valid</param>
    /// <returns>the list of cookies</returns>
    public static Task<List<string>> FetchCookies(this BotContext bot, List<string> domains)
        => bot.ContextCollection.Business.OperationLogic.GetCookies(domains, CancellationToken.None);

    /// <summary>
    /// Fetch the cookies/pskey for accessing other site
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="domains">the domain for the cookie to be valid</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>the list of cookies</returns>
    public static Task<List<string>> FetchCookies(this BotContext bot, List<string> domains, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.GetCookies(domains, cancellationToken);

    /// <summary>
    /// Send the message
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="chain">the chain constructed by <see cref="MessageBuilder"/></param>
    public static Task<MessageResult> SendMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.SendMessage(chain, CancellationToken.None);

    /// <summary>
    /// Send the message
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="chain">the chain constructed by <see cref="MessageBuilder"/></param>
    /// <param name="cancellationToken">The cancellation token</param>
    public static Task<MessageResult> SendMessage(this BotContext bot, MessageChain chain, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.SendMessage(chain, cancellationToken);

    /// <summary>
    /// Recall the group message from Bot itself by <see cref="MessageResult"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group of the message</param>
    /// <param name="result">The return value for <see cref="SendMessage(Lagrange.Core.BotContext,Lagrange.Core.Message.MessageChain)"/></param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallGroupMessage(this BotContext bot, uint groupUin, MessageResult result)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(groupUin, result, CancellationToken.None);

    /// <summary>
    /// Recall the group message from Bot itself by <see cref="MessageResult"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group of the message</param>
    /// <param name="result">The return value for <see cref="SendMessage(Lagrange.Core.BotContext,Lagrange.Core.Message.MessageChain)"/></param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallGroupMessage(this BotContext bot, uint groupUin, MessageResult result, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(groupUin, result, cancellationToken);

    /// <summary>
    /// Recall the group message by <see cref="MessageChain"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="chain">target MessageChain, must be Group</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallGroupMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(chain, CancellationToken.None);

    /// <summary>
    /// Recall the group message by <see cref="MessageChain"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="chain">target MessageChain, must be Group</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallGroupMessage(this BotContext bot, MessageChain chain, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(chain, cancellationToken);

    /// <summary>
    /// Recall the group message by sequence
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group of the message</param>
    /// <param name="sequence">The sequence for target message</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallGroupMessage(this BotContext bot, uint groupUin, uint sequence)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(groupUin, sequence, CancellationToken.None);

    /// <summary>
    /// Recall the group message by sequence
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group of the message</param>
    /// <param name="sequence">The sequence for target message</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallGroupMessage(this BotContext bot, uint groupUin, uint sequence, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(groupUin, sequence, cancellationToken);

    /// <summary>
    /// Recall the group message from Bot itself by <see cref="MessageResult"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="friendUin">The uin for target friend of the message</param>
    /// <param name="result">The return value for <see cref="SendMessage(Lagrange.Core.BotContext,Lagrange.Core.Message.MessageChain)"/></param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallFriendMessage(this BotContext bot, uint friendUin, MessageResult result)
        => bot.ContextCollection.Business.OperationLogic.RecallFriendMessage(friendUin, result, CancellationToken.None);

    /// <summary>
    /// Recall the group message from Bot itself by <see cref="MessageResult"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="friendUin">The uin for target friend of the message</param>
    /// <param name="result">The return value for <see cref="SendMessage(Lagrange.Core.BotContext,Lagrange.Core.Message.MessageChain)"/></param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallFriendMessage(this BotContext bot, uint friendUin, MessageResult result, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.RecallFriendMessage(friendUin, result, cancellationToken);

    /// <summary>
    /// Recall the group message by <see cref="MessageChain"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="chain">target MessageChain, must be Friend</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallFriendMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.RecallFriendMessage(chain, CancellationToken.None);

    /// <summary>
    /// Recall the group message by <see cref="MessageChain"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="chain">target MessageChain, must be Friend</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallFriendMessage(this BotContext bot, MessageChain chain, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.RecallFriendMessage(chain, cancellationToken);

    /// <summary>
    /// Fetch Notifications and requests such as friend requests and Group Join Requests
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <returns></returns>
    public static Task<List<BotGroupRequest>?> FetchGroupRequests(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupRequests(CancellationToken.None);

    /// <summary>
    /// Fetch Notifications and requests such as friend requests and Group Join Requests
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public static Task<List<BotGroupRequest>?> FetchGroupRequests(this BotContext bot, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupRequests(cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    public static Task<List<BotFriendRequest>?> FetchFriendRequests(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.FetchFriendRequests(CancellationToken.None);

    /// <summary>
    ///
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public static Task<List<BotFriendRequest>?> FetchFriendRequests(this BotContext bot, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FetchFriendRequests(cancellationToken);

    /// <summary>
    /// set status
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="status">The status code</param>
    /// <returns></returns>
    public static Task<bool> SetStatus(this BotContext bot, uint status)
        => bot.ContextCollection.Business.OperationLogic.SetStatus(status, CancellationToken.None);

    /// <summary>
    /// set status
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="status">The status code</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public static Task<bool> SetStatus(this BotContext bot, uint status, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.SetStatus(status, cancellationToken);

    /// <summary>
    /// set custom status
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="faceId">faceId that is same as the <see cref="Lagrange.Core.Message.Entity.FaceEntity"/></param>
    /// <param name="text">text that would shown</param>
    /// <returns></returns>
    public static Task<bool> SetCustomStatus(this BotContext bot, uint faceId, string text)
        => bot.ContextCollection.Business.OperationLogic.SetCustomStatus(faceId, text, CancellationToken.None);

    /// <summary>
    /// set custom status
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="faceId">faceId that is same as the <see cref="Lagrange.Core.Message.Entity.FaceEntity"/></param>
    /// <param name="text">text that would shown</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public static Task<bool> SetCustomStatus(this BotContext bot, uint faceId, string text, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.SetCustomStatus(faceId, text, cancellationToken);

    public static Task<bool> GroupTransfer(this BotContext bot, uint groupUin, uint targetUin)
        => bot.ContextCollection.Business.OperationLogic.GroupTransfer(groupUin, targetUin, CancellationToken.None);

    public static Task<bool> GroupTransfer(this BotContext bot, uint groupUin, uint targetUin, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.GroupTransfer(groupUin, targetUin, cancellationToken);

    public static Task<bool> RequestFriend(this BotContext bot, uint targetUin, string question = "", string message = "")
        => bot.ContextCollection.Business.OperationLogic.RequestFriend(targetUin, question, message, CancellationToken.None);

    public static Task<bool> RequestFriend(this BotContext bot, uint targetUin, string question, string message, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.RequestFriend(targetUin, question, message, cancellationToken);

    public static Task<bool> Like(this BotContext bot, uint targetUin, uint count = 1)
        => bot.ContextCollection.Business.OperationLogic.Like(targetUin, count, CancellationToken.None);

    public static Task<bool> Like(this BotContext bot, uint targetUin, uint count, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.Like(targetUin, count, cancellationToken);

    /// <summary>
    /// Get the client key for all sites
    /// </summary>
    public static Task<string?> GetClientKey(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.GetClientKey(CancellationToken.None);

    /// <summary>
    /// Get the client key for all sites
    /// </summary>
    public static Task<string?> GetClientKey(this BotContext bot, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.GetClientKey(cancellationToken);

    /// <summary>
    /// Get the history message record, max 30 seqs
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">target GroupUin</param>
    /// <param name="startSequence">Start Sequence of the message</param>
    /// <param name="endSequence">End Sequence of the message</param>
    public static Task<List<MessageChain>?> GetGroupMessage(this BotContext bot, uint groupUin, uint startSequence, uint endSequence)
        => bot.ContextCollection.Business.OperationLogic.GetGroupMessage(groupUin, startSequence, endSequence, CancellationToken.None);

    /// <summary>
    /// Get the history message record, max 30 seqs
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">target GroupUin</param>
    /// <param name="startSequence">Start Sequence of the message</param>
    /// <param name="endSequence">End Sequence of the message</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public static Task<List<MessageChain>?> GetGroupMessage(this BotContext bot, uint groupUin, uint startSequence, uint endSequence, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.GetGroupMessage(groupUin, startSequence, endSequence, cancellationToken);

    /// <summary>
    /// Get the history message record for private message
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="friendUin">target FriendUin</param>
    /// <param name="timestamp">timestamp of the message chain</param>
    /// <param name="count">number of message to be fetched before timestamp</param>
    public static Task<List<MessageChain>?> GetRoamMessage(this BotContext bot, uint friendUin, uint timestamp, uint count)
        => bot.ContextCollection.Business.OperationLogic.GetRoamMessage(friendUin, timestamp, count, CancellationToken.None);

    /// <summary>
    /// Get the history message record for private message
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="friendUin">target FriendUin</param>
    /// <param name="timestamp">timestamp of the message chain</param>
    /// <param name="count">number of message to be fetched before timestamp</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public static Task<List<MessageChain>?> GetRoamMessage(this BotContext bot, uint friendUin, uint timestamp, uint count, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.GetRoamMessage(friendUin, timestamp, count, cancellationToken);

    /// <summary>
    /// Get the history message record for private message
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="targetChain">target chain</param>
    /// <param name="count">number of message to be fetched before timestamp</param>
    public static Task<List<MessageChain>?> GetRoamMessage(this BotContext bot, MessageChain targetChain, uint count)
    {
        uint timestamp = (uint)new DateTimeOffset(targetChain.Time).ToUnixTimeSeconds();
        return bot.ContextCollection.Business.OperationLogic.GetRoamMessage(targetChain.FriendUin, timestamp, count, CancellationToken.None);
    }

    /// <summary>
    /// Get the history message record for private message
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="targetChain">target chain</param>
    /// <param name="count">number of message to be fetched before timestamp</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public static Task<List<MessageChain>?> GetRoamMessage(this BotContext bot, MessageChain targetChain, uint count, CancellationToken cancellationToken)
    {
        uint timestamp = (uint)new DateTimeOffset(targetChain.Time).ToUnixTimeSeconds();
        return bot.ContextCollection.Business.OperationLogic.GetRoamMessage(targetChain.FriendUin, timestamp, count, cancellationToken);
    }

    public static Task<List<MessageChain>?> GetC2cMessage(this BotContext bot, uint friendUin, uint startSequence, uint endSequence)
    {
        return bot.ContextCollection.Business.OperationLogic.GetC2cMessage(friendUin, startSequence, endSequence, CancellationToken.None);
    }

    public static Task<List<MessageChain>?> GetC2cMessage(this BotContext bot, uint friendUin, uint startSequence, uint endSequence, CancellationToken cancellationToken)
    {
        return bot.ContextCollection.Business.OperationLogic.GetC2cMessage(friendUin, startSequence, endSequence, cancellationToken);
    }

    public static Task<(int code, List<MessageChain>? chains)> GetMessagesByResId(this BotContext bot, string resId)
    {
        return bot.ContextCollection.Business.OperationLogic.GetMessagesByResId(resId, CancellationToken.None);
    }

    public static Task<(int code, List<MessageChain>? chains)> GetMessagesByResId(this BotContext bot, string resId, CancellationToken cancellationToken)
    {
        return bot.ContextCollection.Business.OperationLogic.GetMessagesByResId(resId, cancellationToken);
    }

    /// <summary>
    /// Do group clock in (群打卡)
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">target groupUin</param>
    /// <returns></returns>
    public static Task<BotGroupClockInResult> GroupClockIn(this BotContext bot, uint groupUin)
        => bot.ContextCollection.Business.OperationLogic.GroupClockIn(groupUin, CancellationToken.None);

    /// <summary>
    /// Do group clock in (群打卡)
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">target groupUin</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public static Task<BotGroupClockInResult> GroupClockIn(this BotContext bot, uint groupUin, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.GroupClockIn(groupUin, cancellationToken);

    public static Task<BotUserInfo?> FetchUserInfo(this BotContext bot, uint uin, bool refreshCache = false)
        => bot.ContextCollection.Business.OperationLogic.FetchUserInfo(uin, CancellationToken.None, refreshCache);

    public static Task<BotUserInfo?> FetchUserInfo(this BotContext bot, uint uin, bool refreshCache, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FetchUserInfo(uin, cancellationToken, refreshCache);

    public static Task<List<string>?> FetchCustomFace(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.FetchCustomFace(CancellationToken.None);

    public static Task<List<string>?> FetchCustomFace(this BotContext bot, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FetchCustomFace(cancellationToken);

    public static Task<string?> UploadLongMessage(this BotContext bot, List<MessageChain> chains)
        => bot.ContextCollection.Business.OperationLogic.UploadLongMessage(chains, CancellationToken.None);

    public static Task<string?> UploadLongMessage(this BotContext bot, List<MessageChain> chains, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.UploadLongMessage(chains, cancellationToken);

    public static Task<bool> MarkAsRead(this BotContext bot, MessageChain targetChain)
    {
        uint timestamp = (uint)(targetChain.Time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        return bot.ContextCollection.Business.OperationLogic.MarkAsRead(targetChain.GroupUin ?? 0, targetChain.Uid, targetChain.Sequence, timestamp, CancellationToken.None);
    }

    public static Task<bool> MarkAsRead(this BotContext bot, MessageChain targetChain, CancellationToken cancellationToken)
    {
        uint timestamp = (uint)(targetChain.Time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        return bot.ContextCollection.Business.OperationLogic.MarkAsRead(targetChain.GroupUin ?? 0, targetChain.Uid, targetChain.Sequence, timestamp, cancellationToken);
    }

    public static Task<bool> UploadFriendFile(this BotContext bot, uint targetUin, FileEntity fileEntity)
        => bot.ContextCollection.Business.OperationLogic.UploadFriendFile(targetUin, fileEntity, CancellationToken.None);

    public static Task<bool> UploadFriendFile(this BotContext bot, uint targetUin, FileEntity fileEntity, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.UploadFriendFile(targetUin, fileEntity, cancellationToken);

    public static Task<bool> FriendPoke(this BotContext bot, uint friendUin)
        => bot.ContextCollection.Business.OperationLogic.FriendPoke(friendUin, CancellationToken.None);

    public static Task<bool> FriendPoke(this BotContext bot, uint friendUin, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FriendPoke(friendUin, cancellationToken);

    /// <summary>
    /// Send a special window shake to friend
    /// </summary>
    /// <param name="friendUin">target friend uin</param>
    /// <param name="type">face type</param>
    /// <param name="count">count of face</param>
    public static Task<MessageResult> FriendSpecialShake(this BotContext bot, uint friendUin, SpecialPokeFaceType type, uint count)
        => bot.ContextCollection.Business.OperationLogic.FriendSpecialShake(friendUin, type, count, CancellationToken.None);

    /// <summary>
    /// Send a special window shake to friend
    /// </summary>
    /// <param name="friendUin">target friend uin</param>
    /// <param name="type">face type</param>
    /// <param name="count">count of face</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public static Task<MessageResult> FriendSpecialShake(this BotContext bot, uint friendUin, SpecialPokeFaceType type, uint count, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FriendSpecialShake(friendUin, type, count, cancellationToken);

    /// <summary>
    /// Send a window shake to friend
    /// </summary>
    /// <param name="friendUin">target friend uin</param>
    /// <param name="type">face type</param>
    /// <param name="strength">How big the face will be displayed ([0,3] is valid)</param>
    public static Task<MessageResult> FriendShake(this BotContext bot, uint friendUin, PokeFaceType type, ushort strength)
        => bot.ContextCollection.Business.OperationLogic.FriendShake(friendUin, type, strength, CancellationToken.None);

    /// <summary>
    /// Send a window shake to friend
    /// </summary>
    /// <param name="friendUin">target friend uin</param>
    /// <param name="type">face type</param>
    /// <param name="strength">How big the face will be displayed ([0,3] is valid)</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public static Task<MessageResult> FriendShake(this BotContext bot, uint friendUin, PokeFaceType type, ushort strength, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FriendShake(friendUin, type, strength, cancellationToken);

    public static Task<List<string>?> FetchMarketFaceKey(this BotContext bot, List<string> faceIds)
        => bot.ContextCollection.Business.OperationLogic.FetchMarketFaceKey(faceIds, CancellationToken.None);

    public static Task<List<string>?> FetchMarketFaceKey(this BotContext bot, List<string> faceIds, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.FetchMarketFaceKey(faceIds, cancellationToken);
    
    /// <summary>
    /// Set the avatar of the bot itself
    /// </summary>
    /// <param name="bot">target <see cref="BotContext"/></param>
    /// <param name="avatar">The avatar object, <see cref="ImageEntity"/></param>
    public static Task<bool> SetAvatar(this BotContext bot, ImageEntity avatar)
        => bot.ContextCollection.Business.OperationLogic.SetAvatar(avatar, CancellationToken.None);

    /// <summary>
    /// Set the avatar of the bot itself
    /// </summary>
    /// <param name="bot">target <see cref="BotContext"/></param>
    /// <param name="avatar">The avatar object, <see cref="ImageEntity"/></param>
    /// <param name="cancellationToken">The cancellation token</param>
    public static Task<bool> SetAvatar(this BotContext bot, ImageEntity avatar, CancellationToken cancellationToken)
        => bot.ContextCollection.Business.OperationLogic.SetAvatar(avatar, cancellationToken);
}
