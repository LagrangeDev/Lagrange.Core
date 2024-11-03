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
    /// Recall the group message by sequence
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group of the message</param>
    /// <param name="sequence">The sequence for target message</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallGroupMessage(this BotContext bot, uint groupUin, uint sequence)
        => bot.ContextCollection.Business.OperationLogic.RecallGroupMessage(groupUin, sequence);

    /// <summary>
    /// Recall the group message from Bot itself by <see cref="MessageResult"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="friendUin">The uin for target friend of the message</param>
    /// <param name="result">The return value for <see cref="SendMessage"/></param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallFriendMessage(this BotContext bot, uint friendUin, MessageResult result)
        => bot.ContextCollection.Business.OperationLogic.RecallFriendMessage(friendUin, result);

    /// <summary>
    /// Recall the group message by <see cref="MessageChain"/>
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="chain">target MessageChain, must be Friend</param>
    /// <returns>Successfully recalled or not</returns>
    public static Task<bool> RecallFriendMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.RecallFriendMessage(chain);

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
    public static Task<List<BotFriendRequest>?> FetchFriendRequests(this BotContext bot)
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
        uint timestamp = (uint)new DateTimeOffset(targetChain.Time).ToUnixTimeSeconds();
        return bot.ContextCollection.Business.OperationLogic.GetRoamMessage(targetChain.FriendUin, timestamp, count);
    }

    public static Task<List<MessageChain>?> GetC2cMessage(this BotContext bot, uint friendUin, uint startSequence, uint endSequence)
    {
        return bot.ContextCollection.Business.OperationLogic.GetC2cMessage(friendUin, startSequence, endSequence);
    }

    public static Task<(int code, List<MessageChain>? chains)> GetMessagesByResId(this BotContext bot, string resId)
    {
        return bot.ContextCollection.Business.OperationLogic.GetMessagesByResId(resId);
    }

    /// <summary>
    /// Do group clock in (群打卡)
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">target groupUin</param>
    /// <returns></returns>
    public static Task<BotGroupClockInResult> GroupClockIn(this BotContext bot, uint groupUin)
        => bot.ContextCollection.Business.OperationLogic.GroupClockIn(groupUin);

    public static Task<BotUserInfo?> FetchUserInfo(this BotContext bot, uint uin, bool refreshCache = false)
        => bot.ContextCollection.Business.OperationLogic.FetchUserInfo(uin, refreshCache);

    public static Task<List<string>?> FetchCustomFace(this BotContext bot)
        => bot.ContextCollection.Business.OperationLogic.FetchCustomFace();

    public static Task<string?> UploadLongMessage(this BotContext bot, List<MessageChain> chains)
        => bot.ContextCollection.Business.OperationLogic.UploadLongMessage(chains);

    public static Task<bool> MarkAsRead(this BotContext bot, MessageChain targetChain)
    {
        uint timestamp = (uint)(targetChain.Time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        return bot.ContextCollection.Business.OperationLogic.MarkAsRead(targetChain.GroupUin ?? 0, targetChain.Uid,
            targetChain.Sequence, timestamp);
    }

    public static Task<bool> UploadFriendFile(this BotContext bot, uint targetUin, FileEntity fileEntity)
        => bot.ContextCollection.Business.OperationLogic.UploadFriendFile(targetUin, fileEntity);

    public static Task<bool> FriendPoke(this BotContext bot, uint friendUin)
        => bot.ContextCollection.Business.OperationLogic.FriendPoke(friendUin);

    /// <summary>
    /// Send a special window shake to friend
    /// </summary>
    /// <param name="friendUin">target friend uin</param>
    /// <param name="type">face type</param>
    /// <param name="count">count of face</param>
    public static Task<MessageResult> FriendSpecialShake(this BotContext bot, uint friendUin, SpecialPokeFaceType type, uint count)
        => bot.ContextCollection.Business.OperationLogic.FriendSpecialShake(friendUin, type, count);

    /// <summary>
    /// Send a window shake to friend
    /// </summary>
    /// <param name="friendUin">target friend uin</param>
    /// <param name="type">face type</param>
    /// <param name="strength">How big the face will be displayed ([0,3] is valid)</param>
    public static Task<MessageResult> FriendShake(this BotContext bot, uint friendUin, PokeFaceType type, ushort strength)
        => bot.ContextCollection.Business.OperationLogic.FriendShake(friendUin, type, strength);

    public static Task<List<string>?> FetchMarketFaceKey(this BotContext bot, List<string> faceIds)
        => bot.ContextCollection.Business.OperationLogic.FetchMarketFaceKey(faceIds);

    /// <summary>
    /// Set the avatar of the bot itself
    /// </summary>
    /// <param name="bot">target <see cref="BotContext"/></param>
    /// <param name="avatar">The avatar object, <see cref="ImageEntity"/></param>
    public static Task<bool> SetAvatar(this BotContext bot, ImageEntity avatar)
        => bot.ContextCollection.Business.OperationLogic.SetAvatar(avatar);

    public static Task<bool> FetchSuperFaceId(this BotContext bot, uint id)
        => bot.ContextCollection.Business.OperationLogic.FetchSuperFaceId(id);

    public static Task<SysFaceEntry?> FetchFaceEntity(this BotContext bot, uint id)
        => bot.ContextCollection.Business.OperationLogic.FetchFaceEntity(id);

    public static Task<bool> GroupJoinEmojiChain(this BotContext bot, uint groupUin, uint emojiId, uint targetMessageSeq)
        => bot.ContextCollection.Business.OperationLogic.GroupJoinEmojiChain(groupUin, emojiId, targetMessageSeq);

    public static Task<bool> FriendJoinEmojiChain(this BotContext bot, uint friendUin, uint emojiId,
        uint targetMessageSeq)
        => bot.ContextCollection.Business.OperationLogic.FriendJoinEmojiChain(friendUin, emojiId, targetMessageSeq);

    public static Task<string> UploadImage(this BotContext bot, ImageEntity entity)
        => bot.ContextCollection.Business.OperationLogic.UploadImage(entity);

    public static Task<ImageOcrResult?> OcrImage(this BotContext bot, string url)
        => bot.ContextCollection.Business.OperationLogic.ImageOcr(url);

    public static Task<ImageOcrResult?> OcrImage(this BotContext bot, ImageEntity entity)
        => bot.ContextCollection.Business.OperationLogic.ImageOcr(entity);

    public static Task<(int Code, string ErrMsg, string? Url)> GetGroupGenerateAiRecordUrl(this BotContext bot, uint groupUin, string character, string text, uint chatType)
        => bot.ContextCollection.Business.OperationLogic.GetGroupGenerateAiRecordUrl(groupUin, character, text, chatType);

    public static Task<(int Code, string ErrMsg, RecordEntity? RecordEntity)> GetGroupGenerateAiRecord(this BotContext bot, uint groupUin, string character, string text, uint chatType)
        => bot.ContextCollection.Business.OperationLogic.GetGroupGenerateAiRecord(groupUin, character, text, chatType);

    public static Task<(int Code, string ErrMsg, List<AiCharacterList>? Result)> GetAiCharacters(this BotContext bot, uint chatType, uint groupUin = 42)
        => bot.ContextCollection.Business.OperationLogic.GetAiCharacters(chatType, groupUin);
}