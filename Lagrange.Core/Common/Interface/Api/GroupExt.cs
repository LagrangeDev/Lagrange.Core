using System.Globalization;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Common.Interface.Api;

public static class GroupExt
{
    /// <summary>
    /// Mute the member in the group, Bot must be admin
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group</param>
    /// <param name="targetUin">The uin for target member in such group</param>
    /// <param name="duration">The duration in seconds, 0 for unmute member</param>
    /// <returns>Successfully muted or not</returns>
    public static Task<bool> MuteGroupMember(this BotContext bot, uint groupUin, uint targetUin, uint duration)
        => bot.ContextCollection.Business.OperationLogic.MuteGroupMember(groupUin, targetUin, duration, CancellationToken.None);

    /// <summary>
    /// Mute the member in the group, Bot must be admin
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group</param>
    /// <param name="targetUin">The uin for target member in such group</param>
    /// <param name="duration">The duration in seconds, 0 for unmute member</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>Successfully muted or not</returns>
    public static Task<bool> MuteGroupMember(this BotContext bot, uint groupUin, uint targetUin, uint duration, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.MuteGroupMember(groupUin, targetUin, duration, ct);

    /// <summary>
    /// Mute the group
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group</param>
    /// <param name="isMute">true for mute and false for unmute</param>
    /// <returns>Successfully muted or not</returns>
    public static Task<bool> MuteGroupGlobal(this BotContext bot, uint groupUin, bool isMute)
        => bot.ContextCollection.Business.OperationLogic.MuteGroupGlobal(groupUin, isMute, CancellationToken.None);

    /// <summary>
    /// Mute the group
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group</param>
    /// <param name="isMute">true for mute and false for unmute</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>Successfully muted or not</returns>
    public static Task<bool> MuteGroupGlobal(this BotContext bot, uint groupUin, bool isMute, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.MuteGroupGlobal(groupUin, isMute, ct);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group</param>
    /// <param name="targetUin">The uin for target member in such group</param>
    /// <param name="rejectAddRequest">whether the kicked member can request</param>
    /// <returns>Successfully kicked or not</returns>
    public static Task<bool> KickGroupMember(this BotContext bot, uint groupUin, uint targetUin, bool rejectAddRequest)
        => bot.ContextCollection.Business.OperationLogic.KickGroupMember(groupUin, targetUin, rejectAddRequest, "", CancellationToken.None);


    /// <summary>
    ///
    /// </summary>
    /// <param name="bot">target BotContext</param>
    /// <param name="groupUin">The uin for target group</param>
    /// <param name="targetUin">The uin for target member in such group</param>
    /// <param name="rejectAddRequest">whether the kicked member can request</param>
    /// <param name="ct">The cancellation token</param>
    /// <returns>Successfully kicked or not</returns>
    public static Task<bool> KickGroupMember(this BotContext bot, uint groupUin, uint targetUin, bool rejectAddRequest, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.KickGroupMember(groupUin, targetUin, rejectAddRequest, "", ct);

    public static Task<bool> KickGroupMember(this BotContext bot, uint groupUin, uint targetUin, bool rejectAddRequest, string reason)
        => bot.ContextCollection.Business.OperationLogic.KickGroupMember(groupUin, targetUin, rejectAddRequest, reason, CancellationToken.None);

    public static Task<bool> KickGroupMember(this BotContext bot, uint groupUin, uint targetUin, bool rejectAddRequest, string reason, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.KickGroupMember(groupUin, targetUin, rejectAddRequest, reason, ct);

    public static Task<bool> SetGroupAdmin(this BotContext bot, uint groupUin, uint targetUin, bool isAdmin)
        => bot.ContextCollection.Business.OperationLogic.SetGroupAdmin(groupUin, targetUin, isAdmin, CancellationToken.None);

    public static Task<bool> SetGroupAdmin(this BotContext bot, uint groupUin, uint targetUin, bool isAdmin, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.SetGroupAdmin(groupUin, targetUin, isAdmin, ct);

    public static Task<bool> SetGroupBot(this BotContext bot, uint targetUin, uint On, uint groupUin)
        => bot.ContextCollection.Business.OperationLogic.SetGroupBot(targetUin, On, groupUin, CancellationToken.None);

    public static Task<bool> SetGroupBot(this BotContext bot, uint targetUin, uint On, uint groupUin, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.SetGroupBot(targetUin, On, groupUin, ct);

    [Obsolete("Cosider using SetGroupBotHD(BotContext, uint, uint, string?, string?) instead")]
    public static Task<bool> SetGroupBotHD(this BotContext bot, uint targetUin, uint groupUin)
        => bot.SetGroupBotHD(targetUin, groupUin, null, null);
    public static Task<bool> SetGroupBotHD(this BotContext bot, uint targetUin, uint groupUin, string? data_1, string? data_2)
        => bot.ContextCollection.Business.OperationLogic.SetGroupBotHD(targetUin, groupUin, data_1, data_2, CancellationToken.None);
    public static Task<bool> SetGroupBotHD(this BotContext bot, uint targetUin, uint groupUin, string? data_1, string? data_2, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.SetGroupBotHD(targetUin, groupUin, data_1, data_2, ct);

    public static Task<bool> RenameGroupMember(this BotContext bot, uint groupUin, uint targetUin, string targetName)
        => bot.ContextCollection.Business.OperationLogic.RenameGroupMember(groupUin, targetUin, targetName, CancellationToken.None);

    public static Task<bool> RenameGroupMember(this BotContext bot, uint groupUin, uint targetUin, string targetName, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.RenameGroupMember(groupUin, targetUin, targetName, ct);

    public static Task<bool> RenameGroup(this BotContext bot, uint groupUin, string targetName)
        => bot.ContextCollection.Business.OperationLogic.RenameGroup(groupUin, targetName, CancellationToken.None);

    public static Task<bool> RenameGroup(this BotContext bot, uint groupUin, string targetName, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.RenameGroup(groupUin, targetName, ct);

    public static Task<bool> RemarkGroup(this BotContext bot, uint groupUin, string targetRemark)
        => bot.ContextCollection.Business.OperationLogic.RemarkGroup(groupUin, targetRemark, CancellationToken.None);

    public static Task<bool> RemarkGroup(this BotContext bot, uint groupUin, string targetRemark, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.RemarkGroup(groupUin, targetRemark, ct);

    public static Task<bool> LeaveGroup(this BotContext bot, uint groupUin)
        => bot.ContextCollection.Business.OperationLogic.LeaveGroup(groupUin, CancellationToken.None);

    public static Task<bool> LeaveGroup(this BotContext bot, uint groupUin, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.LeaveGroup(groupUin, ct);

    public static Task<bool> InviteGroup(this BotContext bot, uint groupUin, Dictionary<uint, uint?> invitedUins)
        => bot.ContextCollection.Business.OperationLogic.InviteGroup(groupUin, invitedUins, CancellationToken.None);

    public static Task<bool> InviteGroup(this BotContext bot, uint groupUin, Dictionary<uint, uint?> invitedUins, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.InviteGroup(groupUin, invitedUins, ct);

    public static Task<bool> SetGroupRequest(this BotContext bot, BotGroupRequest request, bool accept = true, string reason = "")
        => bot.ContextCollection.Business.OperationLogic.SetGroupRequest(request.GroupUin, request.Sequence, (uint)request.EventType, accept, reason, CancellationToken.None);

    public static Task<bool> SetGroupRequest(this BotContext bot, BotGroupRequest request, CancellationToken ct, bool accept = true, string reason = "")
        => bot.ContextCollection.Business.OperationLogic.SetGroupRequest(request.GroupUin, request.Sequence, (uint)request.EventType, accept, reason, ct);

    public static Task<bool> SetGroupFilteredRequest(this BotContext bot, BotGroupRequest request, bool accept = true, string reason = "")
        => bot.ContextCollection.Business.OperationLogic.SetGroupFilteredRequest(request.GroupUin, request.Sequence, (uint)request.EventType, accept, reason, CancellationToken.None);

    public static Task<bool> SetGroupFilteredRequest(this BotContext bot, BotGroupRequest request, CancellationToken ct, bool accept = true, string reason = "")
        => bot.ContextCollection.Business.OperationLogic.SetGroupFilteredRequest(request.GroupUin, request.Sequence, (uint)request.EventType, accept, reason, ct);

    public static Task<bool> SetFriendRequest(this BotContext bot, FriendRequestEvent request, bool accept = true)
        => bot.ContextCollection.Business.OperationLogic.SetFriendRequest(request.SourceUid, accept, CancellationToken.None);

    public static Task<bool> SetFriendRequest(this BotContext bot, FriendRequestEvent request, CancellationToken ct, bool accept = true)
        => bot.ContextCollection.Business.OperationLogic.SetFriendRequest(request.SourceUid, accept, ct);

    public static Task<bool> GroupPoke(this BotContext bot, uint groupUin, uint friendUin)
        => bot.ContextCollection.Business.OperationLogic.GroupPoke(groupUin, friendUin, CancellationToken.None);

    public static Task<bool> GroupPoke(this BotContext bot, uint groupUin, uint friendUin, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.GroupPoke(groupUin, friendUin, ct);

    public static Task<bool> SetEssenceMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.SetEssenceMessage(chain.GroupUin ?? 0, chain.Sequence, (uint)(chain.MessageId & 0xFFFFFFFF), CancellationToken.None);

    public static Task<bool> SetEssenceMessage(this BotContext bot, MessageChain chain, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.SetEssenceMessage(chain.GroupUin ?? 0, chain.Sequence, (uint)(chain.MessageId & 0xFFFFFFFF), ct);

    public static Task<bool> RemoveEssenceMessage(this BotContext bot, MessageChain chain)
        => bot.ContextCollection.Business.OperationLogic.RemoveEssenceMessage(chain.GroupUin ?? 0, chain.Sequence, (uint)(chain.MessageId & 0xFFFFFFFF), CancellationToken.None);

    public static Task<bool> RemoveEssenceMessage(this BotContext bot, MessageChain chain, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.RemoveEssenceMessage(chain.GroupUin ?? 0, chain.Sequence, (uint)(chain.MessageId & 0xFFFFFFFF), ct);

    public static Task<bool> GroupSetSpecialTitle(this BotContext bot, uint groupUin, uint targetUin, string title)
        => bot.ContextCollection.Business.OperationLogic.GroupSetSpecialTitle(groupUin, targetUin, title, CancellationToken.None);

    public static Task<bool> GroupSetSpecialTitle(this BotContext bot, uint groupUin, uint targetUin, string title, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.GroupSetSpecialTitle(groupUin, targetUin, title, ct);

    public static Task<bool> GroupSetMessageReaction(this BotContext bot, uint groupUin, uint sequence, string code)
        => bot.ContextCollection.Business.OperationLogic.SetMessageReaction(groupUin, sequence, code, true, CancellationToken.None);

    public static Task<bool> GroupSetMessageReaction(this BotContext bot, uint groupUin, uint sequence, string code, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.SetMessageReaction(groupUin, sequence, code, true, ct);

    public static Task<bool> GroupSetMessageReaction(this BotContext bot, uint groupUin, uint sequence, string code, bool isSet)
        => bot.ContextCollection.Business.OperationLogic.SetMessageReaction(groupUin, sequence, code, isSet, CancellationToken.None);

    public static Task<bool> GroupSetMessageReaction(this BotContext bot, uint groupUin, uint sequence, string code, bool isSet, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.SetMessageReaction(groupUin, sequence, code, isSet, ct);

    public static Task<bool> GroupSetAvatar(this BotContext bot, uint groupUin, ImageEntity imageEntity)
        => bot.ContextCollection.Business.OperationLogic.GroupSetAvatar(groupUin, imageEntity, CancellationToken.None);
    
    public static Task<bool> GroupSetAvatar(this BotContext bot, uint groupUin, ImageEntity imageEntity, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.GroupSetAvatar(groupUin, imageEntity, ct);

    #region Group File System

    public static Task<ulong> FetchGroupFSSpace(this BotContext bot, uint groupUin)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSSpace(groupUin, CancellationToken.None);

    public static Task<ulong> FetchGroupFSSpace(this BotContext bot, uint groupUin, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSSpace(groupUin, ct);

    public static Task<uint> FetchGroupFSCount(this BotContext bot, uint groupUin)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSCount(groupUin, CancellationToken.None);

    public static Task<uint> FetchGroupFSCount(this BotContext bot, uint groupUin, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSCount(groupUin, ct);

    public static Task<List<IBotFSEntry>> FetchGroupFSList(this BotContext bot, uint groupUin, string targetDirectory = "/")
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSList(groupUin, targetDirectory, CancellationToken.None);

    public static Task<List<IBotFSEntry>> FetchGroupFSList(this BotContext bot, uint groupUin, CancellationToken ct, string targetDirectory = "/")
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSList(groupUin, targetDirectory, ct);

    public static Task<string> FetchGroupFSDownload(this BotContext bot, uint groupUin, string fileId)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSDownload(groupUin, fileId, CancellationToken.None);

    public static Task<string> FetchGroupFSDownload(this BotContext bot, uint groupUin, string fileId, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSDownload(groupUin, fileId, ct);

    public static Task<(int RetCode, string RetMsg)> GroupFSMove(this BotContext bot, uint groupUin, string fileId, string parentDirectory, string targetDirectory)
        => bot.ContextCollection.Business.OperationLogic.GroupFSMove(groupUin, fileId, parentDirectory, targetDirectory, CancellationToken.None);

    public static Task<(int RetCode, string RetMsg)> GroupFSMove(this BotContext bot, uint groupUin, string fileId, string parentDirectory, string targetDirectory, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.GroupFSMove(groupUin, fileId, parentDirectory, targetDirectory, ct);

    public static Task<(int RetCode, string RetMsg)> GroupFSDelete(this BotContext bot, uint groupUin, string fileId)
        => bot.ContextCollection.Business.OperationLogic.GroupFSDelete(groupUin, fileId, CancellationToken.None);

    public static Task<(int RetCode, string RetMsg)> GroupFSDelete(this BotContext bot, uint groupUin, string fileId, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.GroupFSDelete(groupUin, fileId, ct);

    public static Task<(int RetCode, string RetMsg)> GroupFSCreateFolder(this BotContext bot, uint groupUin, string name)
        => bot.ContextCollection.Business.OperationLogic.GroupFSCreateFolder(groupUin, name, CancellationToken.None);

    public static Task<(int RetCode, string RetMsg)> GroupFSCreateFolder(this BotContext bot, uint groupUin, string name, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.GroupFSCreateFolder(groupUin, name, ct);

    public static Task<(int RetCode, string RetMsg)> GroupFSDeleteFolder(this BotContext bot, uint groupUin, string folderId)
        => bot.ContextCollection.Business.OperationLogic.GroupFSDeleteFolder(groupUin, folderId, CancellationToken.None);

    public static Task<(int RetCode, string RetMsg)> GroupFSDeleteFolder(this BotContext bot, uint groupUin, string folderId, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.GroupFSDeleteFolder(groupUin, folderId, ct);

    public static Task<(int RetCode, string RetMsg)> GroupFSRenameFolder(this BotContext bot, uint groupUin, string folderId, string newFolderName)
        => bot.ContextCollection.Business.OperationLogic.GroupFSRenameFolder(groupUin, folderId, newFolderName, CancellationToken.None);

    public static Task<(int RetCode, string RetMsg)> GroupFSRenameFolder(this BotContext bot, uint groupUin, string folderId, string newFolderName, CancellationToken ct)
        => bot.ContextCollection.Business.OperationLogic.GroupFSRenameFolder(groupUin, folderId, newFolderName, ct);

    public static Task<bool> GroupFSUpload(this BotContext bot, uint groupUin, FileEntity fileEntity, string targetDirectory = "/")
        => bot.ContextCollection.Business.OperationLogic.GroupFSUpload(groupUin, fileEntity, targetDirectory, CancellationToken.None);

    public static Task<bool> GroupFSUpload(this BotContext bot, uint groupUin, FileEntity fileEntity, CancellationToken ct, string targetDirectory = "/")
        => bot.ContextCollection.Business.OperationLogic.GroupFSUpload(groupUin, fileEntity, targetDirectory, ct);

    #endregion
}