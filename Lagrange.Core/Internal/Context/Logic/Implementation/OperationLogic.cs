using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Context.Attributes;
using Lagrange.Core.Internal.Context.Uploader;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Highway;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Context.Logic.Implementation;

[BusinessLogic("OperationLogic", "Manage the user operation of the bot")]
internal class OperationLogic : LogicBase
{
    private const string Tag = nameof(OperationLogic);

    internal OperationLogic(ContextCollection collection) : base(collection) { }

    public async Task<List<string>> GetCookies(List<string> domains)
    {
        var fetchCookieEvent = FetchCookieEvent.Create(domains);
        var events = await Collection.Business.SendEvent(fetchCookieEvent);
        return events.Count != 0 ? ((FetchCookieEvent)events[0]).Cookies : new List<string>();
    }

    public Task<List<BotFriend>> FetchFriends(bool refreshCache = false) =>
        Collection.Business.CachingLogic.GetCachedFriends(refreshCache);

    public Task<List<BotGroupMember>> FetchMembers(uint groupUin, bool refreshCache = false) =>
        Collection.Business.CachingLogic.GetCachedMembers(groupUin, refreshCache);

    public Task<List<BotGroup>> FetchGroups(bool refreshCache) =>
        Collection.Business.CachingLogic.GetCachedGroups(refreshCache);

    public async Task<MessageResult> SendMessage(MessageChain chain)
    {
        uint clientSeq = chain.ClientSequence;
        ulong messageId = chain.MessageId;

        var sendMessageEvent = SendMessageEvent.Create(chain);
        var events = await Collection.Business.SendEvent(sendMessageEvent);
        if (events.Count == 0) return new MessageResult { Result = 9057 };

        var result = ((SendMessageEvent)events[0]).MsgResult;
        result.ClientSequence = clientSeq;
        result.MessageId = messageId;
        return result;
    }

    public async Task<bool> MuteGroupMember(uint groupUin, uint targetUin, uint duration)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin);
        if (uid == null) return false;

        var muteGroupMemberEvent = GroupMuteMemberEvent.Create(groupUin, duration, uid);
        var events = await Collection.Business.SendEvent(muteGroupMemberEvent);
        return events.Count != 0 && ((GroupMuteMemberEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> MuteGroupGlobal(uint groupUin, bool isMute)
    {
        var muteGroupMemberEvent = GroupMuteGlobalEvent.Create(groupUin, isMute);
        var events = await Collection.Business.SendEvent(muteGroupMemberEvent);
        return events.Count != 0 && ((GroupMuteGlobalEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> KickGroupMember(uint groupUin, uint targetUin, bool rejectAddRequest, string reason)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin);
        if (uid == null) return false;

        var muteGroupMemberEvent = GroupKickMemberEvent.Create(groupUin, uid, rejectAddRequest, reason);
        var events = await Collection.Business.SendEvent(muteGroupMemberEvent);
        return events.Count != 0 && ((GroupKickMemberEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> SetGroupAdmin(uint groupUin, uint targetUin, bool isAdmin)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin);
        if (uid == null) return false;

        var setGroupAdminEvent = GroupSetAdminEvent.Create(groupUin, uid, isAdmin);
        var events = await Collection.Business.SendEvent(setGroupAdminEvent);
        return events.Count != 0 && ((GroupSetAdminEvent)events[0]).ResultCode == 0;
    }

    public async Task<(int, string?)> SetGroupTodo(uint groupUin, uint sequence)
    {
        var setGroupTodoEvent = GroupSetTodoEvent.Create(groupUin, sequence);
        var events = await Collection.Business.SendEvent(setGroupTodoEvent);

        if (events.Count == 0) return (-1, "No Events");

        var @event = (GroupSetTodoEvent)events[0];

        return (@event.ResultCode, @event.ResultMessage);
    }

    public async Task<(int, string?)> RemoveGroupTodo(uint groupUin)
    {
        var setGroupTodoEvent = GroupRemoveTodoEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(setGroupTodoEvent);

        if (events.Count == 0) return (-1, "No Event");

        var @event = (GroupRemoveTodoEvent)events[0];

        return (@event.ResultCode, @event.ResultMessage);
    }

    public async Task<(int, string?)> FinishGroupTodo(uint groupUin)
    {
        var setGroupTodoEvent = GroupFinishTodoEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(setGroupTodoEvent);

        if (events.Count == 0) return (-1, "No Event");

        var @event = (GroupFinishTodoEvent)events[0];

        return (@event.ResultCode, @event.ResultMessage);
    }

    public async Task<BotGetGroupTodoResult> GetGroupTodo(uint groupUin)
    {
        var setGroupTodoEvent = GroupGetTodoEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(setGroupTodoEvent);

        if (events.Count == 0) return new(-1, "No Event", 0, 0, string.Empty);

        var @event = (GroupGetTodoEvent)events[0];

        return new(
            @event.ResultCode,
            @event.ResultMessage,
            @event.GroupUin,
            @event.Sequence,
            @event.Preview
        );
    }

    public async Task<bool> SetGroupBot(uint BotId, uint On, uint groupUin)
    {
        var muteBotEvent = GroupSetBotEvent.Create(BotId, On, groupUin);
        var events = await Collection.Business.SendEvent(muteBotEvent);
        return events.Count != 0 && ((GroupSetBotEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> SetGroupBotHD(uint BotId, uint groupUin, string? data_1, string? data_2)
    {
        var muteBotEvent = GroupSetBothdEvent.Create(BotId, groupUin, data_1, data_2);
        var events = await Collection.Business.SendEvent(muteBotEvent);
        return events.Count != 0 && ((GroupSetBothdEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RenameGroupMember(uint groupUin, uint targetUin, string targetName)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin);
        if (uid == null) return false;

        var renameGroupEvent = RenameMemberEvent.Create(groupUin, uid, targetName);
        var events = await Collection.Business.SendEvent(renameGroupEvent);
        return events.Count != 0 && ((RenameMemberEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RenameGroup(uint groupUin, string targetName)
    {
        var renameGroupEvent = GroupRenameEvent.Create(groupUin, targetName);
        var events = await Collection.Business.SendEvent(renameGroupEvent);
        return events.Count != 0 && ((GroupRenameEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RemarkGroup(uint groupUin, string targetRemark)
    {
        var renameGroupEvent = GroupRemarkEvent.Create(groupUin, targetRemark);
        var events = await Collection.Business.SendEvent(renameGroupEvent);
        return events.Count != 0 && ((GroupRemarkEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> LeaveGroup(uint groupUin)
    {
        var leaveGroupEvent = GroupLeaveEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(leaveGroupEvent);
        return events.Count != 0 && ((GroupLeaveEvent)events[0]).ResultCode == 0;
    }

    public async Task<ulong> FetchGroupFSSpace(uint groupUin)
    {
        var groupFSSpaceEvent = GroupFSSpaceEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(groupFSSpaceEvent);
        return ((GroupFSSpaceEvent)events[0]).TotalSpace - ((GroupFSSpaceEvent)events[0]).UsedSpace;
    }

    public async Task<uint> FetchGroupFSCount(uint groupUin)
    {
        var groupFSSpaceEvent = GroupFSCountEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(groupFSSpaceEvent);
        return ((GroupFSCountEvent)events[0]).FileCount;
    }

    public async Task<List<IBotFSEntry>> FetchGroupFSList(uint groupUin, string targetDirectory)
    {
        uint startIndex = 0;
        var entries = new List<IBotFSEntry>();
        while (true)
        {
            var groupFSListEvent = GroupFSListEvent.Create(groupUin, targetDirectory, startIndex, 20);
            var events = await Collection.Business.SendEvent(groupFSListEvent);
            if (events.Count == 0) break;
            entries.AddRange(((GroupFSListEvent)events[0]).FileEntries);
            if (((GroupFSListEvent)events[0]).IsEnd) break;
            startIndex += 20;
        }

        return entries;
    }

    public async Task<string> FetchGroupFSDownload(uint groupUin, string fileId)
    {
        var groupFSDownloadEvent = GroupFSDownloadEvent.Create(groupUin, fileId);
        var events = await Collection.Business.SendEvent(groupFSDownloadEvent);
        return $"{((GroupFSDownloadEvent)events[0]).FileUrl}{fileId}";
    }

    public async Task<(int, string)> GroupFSMove(uint groupUin, string fileId, string parentDirectory, string targetDirectory)
    {
        var groupFSMoveEvent = GroupFSMoveEvent.Create(groupUin, fileId, parentDirectory, targetDirectory);
        var events = await Collection.Business.SendEvent(groupFSMoveEvent);
        int retCode = events.Count > 0 ? ((GroupFSMoveEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSMoveEvent)events[0]).RetMsg : string.Empty;
        return (retCode, retMsg);
    }

    public async Task<(int, string)> GroupFSDelete(uint groupUin, string fileId)
    {
        var groupFSDeleteEvent = GroupFSDeleteEvent.Create(groupUin, fileId);
        var events = await Collection.Business.SendEvent(groupFSDeleteEvent);
        int retCode = events.Count > 0 ? ((GroupFSDeleteEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSDeleteEvent)events[0]).RetMsg : string.Empty;
        return (retCode, retMsg);
    }

    public async Task<(int, string)> GroupFSCreateFolder(uint groupUin, string name)
    {
        var groupFSCreateFolderEvent = GroupFSCreateFolderEvent.Create(groupUin, name);
        var events = await Collection.Business.SendEvent(groupFSCreateFolderEvent);
        int retCode = events.Count > 0 ? ((GroupFSCreateFolderEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSCreateFolderEvent)events[0]).RetMsg : string.Empty;
        return (retCode, retMsg);
    }

    public async Task<(int, string)> GroupFSDeleteFolder(uint groupUin, string folderId)
    {
        var groupFSDeleteFolderEvent = GroupFSDeleteFolderEvent.Create(groupUin, folderId);
        var events = await Collection.Business.SendEvent(groupFSDeleteFolderEvent);
        int retCode = events.Count > 0 ? ((GroupFSDeleteFolderEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSDeleteFolderEvent)events[0]).RetMsg : string.Empty;
        return (retCode, retMsg);
    }

    public async Task<(int, string)> GroupFSRenameFolder(uint groupUin, string folderId, string newFolderName)
    {
        var groupFSDeleteFolderEvent = GroupFSRenameFolderEvent.Create(groupUin, folderId, newFolderName);
        var events = await Collection.Business.SendEvent(groupFSDeleteFolderEvent);
        int retCode = events.Count > 0 ? ((GroupFSRenameFolderEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSRenameFolderEvent)events[0]).RetMsg : "";
        return (retCode, retMsg);
    }

    public Task<bool> GroupFSUpload(uint groupUin, FileEntity fileEntity, string targetDirectory)
    {
        try
        {
            return FileUploader.UploadGroup(Collection, MessageBuilder.Group(groupUin).Build(), fileEntity, targetDirectory);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public async Task<bool> UploadFriendFile(uint targetUin, FileEntity fileEntity)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(null, targetUin);
        var chain = new MessageChain(targetUin, Collection.Keystore.Uid ?? "", uid ?? "") { fileEntity };

        try
        {
            return await FileUploader.UploadPrivate(Collection, chain, fileEntity);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RecallGroupMessage(uint groupUin, MessageResult result)
    {
        if (result.Sequence == null) return false;

        var recallMessageEvent = RecallGroupMessageEvent.Create(groupUin, result.Sequence.Value);
        var events = await Collection.Business.SendEvent(recallMessageEvent);
        return events.Count != 0 && ((RecallGroupMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RecallGroupMessage(MessageChain chain)
    {
        if (chain.GroupUin == null) return false;

        var recallMessageEvent = RecallGroupMessageEvent.Create(chain.GroupUin.Value, chain.Sequence);
        var events = await Collection.Business.SendEvent(recallMessageEvent);
        return events.Count != 0 && ((RecallGroupMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RecallGroupMessage(uint groupUin, uint sequence)
    {
        var recallMessageEvent = RecallGroupMessageEvent.Create(groupUin, sequence);
        var events = await Collection.Business.SendEvent(recallMessageEvent);
        return events.Count != 0 && ((RecallGroupMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RecallFriendMessage(uint friendUin, MessageResult result)
    {
        if (result.Sequence == null) return false;
        if (await Collection.Business.CachingLogic.ResolveUid(null, friendUin) is not { } uid) return false;

        var recallMessageEvent = RecallFriendMessageEvent.Create(uid, result.ClientSequence, result.Sequence ?? 0, (uint)(result.MessageId & uint.MaxValue), result.Timestamp);
        var events = await Collection.Business.SendEvent(recallMessageEvent);
        return events.Count != 0 && ((RecallFriendMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RecallFriendMessage(MessageChain chain)
    {
        if (await Collection.Business.CachingLogic.ResolveUid(null, chain.TargetUin) is not { } uid) return false;

        uint timestamp = (uint)new DateTimeOffset(chain.Time).ToUnixTimeSeconds();
        var recallMessageEvent = RecallFriendMessageEvent.Create(uid, chain.ClientSequence, chain.Sequence, (uint)(chain.MessageId & uint.MaxValue), timestamp);
        var events = await Collection.Business.SendEvent(recallMessageEvent);
        return events.Count != 0 && ((RecallFriendMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<List<BotGroupRequest>?> FetchGroupRequests()
    {
        var fetchRequestsEvent = FetchGroupRequestsEvent.Create();
        var events = await Collection.Business.SendEvent(fetchRequestsEvent);
        if (events.Count == 0) return null;

        var resolved = events.Cast<FetchGroupRequestsEvent>().SelectMany(e => e.Events).ToList();
        var results = new List<BotGroupRequest>();

        foreach (var result in resolved)
        {
            var uins = await Task.WhenAll(ResolveUid(result.InvitorMemberUid), ResolveUid(result.TargetMemberUid),
                ResolveUid(result.OperatorUid));
            uint invitorUin = uins[0];
            uint targetUin = uins[1];
            uint operatorUin = uins[2];

            results.Add(new BotGroupRequest(
                result.GroupUin,
                invitorUin,
                result.InvitorMemberCard,
                targetUin,
                result.TargetMemberCard,
                operatorUin,
                result.OperatorName,
                result.State,
                result.Sequence,
                result.EventType,
                result.Comment,
                result.IsFiltered
            ));
        }

        return results;

        async Task<uint> ResolveUid(string? uid)
        {
            if (uid == null) return 0;

            var fetchUidEvent = FetchUserInfoEvent.Create(uid);
            var e = await Collection.Business.SendEvent(fetchUidEvent);
            return e.Count == 0 ? 0 : ((FetchUserInfoEvent)e[0]).UserInfo.Uin;
        }
    }

    public async Task<List<BotFriendRequest>?> FetchFriendRequests()
    {
        var fetchRequestsEvent = FetchFriendsRequestsEvent.Create();
        var events = await Collection.Business.SendEvent(fetchRequestsEvent);
        if (events.Count == 0) return null;

        var resolved = ((FetchFriendsRequestsEvent)events[0]).Requests;
        foreach (var result in resolved)
        {
            var uins = await Task.WhenAll(ResolveUid(result.TargetUid), ResolveUid(result.SourceUid));
            result.TargetUin = uins[0];
            result.SourceUin = uins[1];
        }

        return resolved;

        async Task<uint> ResolveUid(string? uid)
        {
            if (uid == null) return 0;

            var fetchUidEvent = FetchUserInfoEvent.Create(uid);
            var e = await Collection.Business.SendEvent(fetchUidEvent);
            return e.Count == 0 ? 0 : ((FetchUserInfoEvent)e[0]).UserInfo.Uin;
        }
    }

    public async Task<bool> GroupTransfer(uint groupUin, uint targetUin)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin);
        if (uid == null || Collection.Keystore.Uid is not { } source) return false;

        var transferEvent = GroupTransferEvent.Create(groupUin, source, uid);
        var results = await Collection.Business.SendEvent(transferEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> SetStatus(uint status)
    {
        var setStatusEvent = SetStatusEvent.Create(status, 0);
        var results = await Collection.Business.SendEvent(setStatusEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> SetCustomStatus(uint faceId, string text)
    {
        var setCustomStatusEvent = SetCustomStatusEvent.Create(faceId, text);
        var results = await Collection.Business.SendEvent(setCustomStatusEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> DeleteFriend(uint targetUin, bool block)
    {
        var uid = await Collection.Business.CachingLogic.ResolveUid(null, targetUin);
        var deleteFriendEvent = DeleteFriendEvent.Create(uid, block);
        var results = await Collection.Business.SendEvent(deleteFriendEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> RequestFriend(uint targetUin, string question, string message)
    {
        var requestFriendSearchEvent = RequestFriendSearchEvent.Create(targetUin);
        var searchEvents = await Collection.Business.SendEvent(requestFriendSearchEvent);
        if (searchEvents.Count == 0) return false;
        await Task.Delay(5000);

        var requestFriendSettingEvent = RequestFriendSettingEvent.Create(targetUin);
        var settingEvents = await Collection.Business.SendEvent(requestFriendSettingEvent);
        if (settingEvents.Count == 0) return false;

        var requestFriendEvent = RequestFriendEvent.Create(targetUin, message, question);
        var events = await Collection.Business.SendEvent(requestFriendEvent);
        return events.Count != 0 && ((RequestFriendEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> Like(uint targetUin, uint count)
    {
        var uid = await Collection.Business.CachingLogic.ResolveUid(null, targetUin);
        if (uid == null) return false;

        var friendLikeEvent = FriendLikeEvent.Create(uid, count);
        var results = await Collection.Business.SendEvent(friendLikeEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> InviteGroup(uint targetGroupUin, Dictionary<uint, uint?> invitedUins)
    {
        var invitedUids = new Dictionary<string, uint?>(invitedUins.Count);
        foreach (var (friendUin, groupUin) in invitedUins)
        {
            string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, friendUin);
            if (uid != null) invitedUids[uid] = groupUin;
        }

        var @event = GroupInviteEvent.Create(targetGroupUin, invitedUids);
        var results = await Collection.Business.SendEvent(@event);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<string?> GetClientKey()
    {
        var clientKeyEvent = FetchClientKeyEvent.Create();
        var events = await Collection.Business.SendEvent(clientKeyEvent);
        return events.Count == 0 ? null : ((FetchClientKeyEvent)events[0]).ClientKey;
    }

    public async Task<bool> SetGroupRequest(uint groupUin, ulong sequence, uint type, bool accept, string reason)
    {
        var inviteEvent = SetGroupRequestEvent.Create(accept, groupUin, sequence, type, reason);
        var results = await Collection.Business.SendEvent(inviteEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> SetGroupFilteredRequest(uint groupUin, ulong sequence, uint type, bool accept, string reason)
    {
        var inviteEvent = SetGroupFilteredRequestEvent.Create(accept, groupUin, sequence, type, reason);
        var results = await Collection.Business.SendEvent(inviteEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> SetFriendRequest(string targetUid, bool accept)
    {
        var inviteEvent = SetFriendRequestEvent.Create(targetUid, accept);
        var results = await Collection.Business.SendEvent(inviteEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<List<MessageChain>?> GetGroupMessage(uint groupUin, uint startSequence, uint endSequence)
    {
        var getMsgEvent = GetGroupMessageEvent.Create(groupUin, startSequence, endSequence);
        var results = await Collection.Business.SendEvent(getMsgEvent);
        return results.Count != 0 ? ((GetGroupMessageEvent)results[0]).Chains : null;
    }

    public async Task<List<MessageChain>?> GetRoamMessage(uint friendUin, uint time, uint count)
    {
        if (await Collection.Business.CachingLogic.ResolveUid(null, friendUin) is not { } uid) return null;

        var roamEvent = GetRoamMessageEvent.Create(uid, time, count);
        var results = await Collection.Business.SendEvent(roamEvent);
        return results.Count != 0 ? ((GetRoamMessageEvent)results[0]).Chains : null;
    }

    public async Task<List<MessageChain>?> GetC2cMessage(uint friendUin, uint startSequence, uint endSequence)
    {
        if (await Collection.Business.CachingLogic.ResolveUid(null, friendUin) is not { } uid) return null;

        var c2cEvent = GetC2cMessageEvent.Create(uid, startSequence, endSequence);
        var results = await Collection.Business.SendEvent(c2cEvent);
        return results.Count != 0 ? ((GetC2cMessageEvent)results[0]).Chains : null;
    }

    public async Task<(int code, List<MessageChain>? chains)> GetMessagesByResId(string resId)
    {
        var @event = MultiMsgDownloadEvent.Create(Collection.Keystore.Uid ?? "", resId);
        var results = await Collection.Business.SendEvent(@event);

        if (results.Count == 0) return (-9999, null);
        var result = (MultiMsgDownloadEvent)results[0];

        return (result.ResultCode, result.Chains);
    }

    public async Task<List<string>?> FetchCustomFace()
    {
        var fetchCustomFaceEvent = FetchCustomFaceEvent.Create();
        var results = await Collection.Business.SendEvent(fetchCustomFaceEvent);
        return results.Count != 0 ? ((FetchCustomFaceEvent)results[0]).Urls : null;
    }

    public async Task<string?> UploadLongMessage(List<MessageChain> chains)
    {
        var multiMsgUploadEvent = MultiMsgUploadEvent.Create(null, chains);
        var results = await Collection.Business.SendEvent(multiMsgUploadEvent);
        return results.Count != 0 ? ((MultiMsgUploadEvent)results[0]).ResId : null;
    }

    public async Task<bool> MarkAsRead(uint groupUin, string? targetUid, uint startSequence, uint time)
    {
        var markAsReadEvent = MarkReadedEvent.Create(groupUin, targetUid, startSequence, time);
        var results = await Collection.Business.SendEvent(markAsReadEvent);
        return results.Count != 0 && ((MarkReadedEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> FriendPoke(uint friendUin)
    {
        var friendPokeEvent = FriendPokeEvent.Create(friendUin);
        var results = await Collection.Business.SendEvent(friendPokeEvent);
        return results.Count != 0 && ((FriendPokeEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> GroupPoke(uint groupUin, uint friendUin)
    {
        var friendPokeEvent = GroupPokeEvent.Create(friendUin, groupUin);
        var results = await Collection.Business.SendEvent(friendPokeEvent);
        return results.Count != 0 && ((FriendPokeEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> SetEssenceMessage(uint groupUin, uint sequence, uint random)
    {
        var setEssenceMessageEvent = SetEssenceMessageEvent.Create(groupUin, sequence, random);
        var results = await Collection.Business.SendEvent(setEssenceMessageEvent);
        return results.Count != 0 && ((SetEssenceMessageEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> RemoveEssenceMessage(uint groupUin, uint sequence, uint random)
    {
        var removeEssenceMessageEvent = RemoveEssenceMessageEvent.Create(groupUin, sequence, random);
        var results = await Collection.Business.SendEvent(removeEssenceMessageEvent);
        return results.Count != 0 && ((RemoveEssenceMessageEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> GroupSetSpecialTitle(uint groupUin, uint targetUin, string title)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin);
        if (uid == null) return false;

        var groupSetSpecialTitleEvent = GroupSetSpecialTitleEvent.Create(groupUin, uid, title);
        var events = await Collection.Business.SendEvent(groupSetSpecialTitleEvent);
        return events.Count != 0 && ((GroupSetSpecialTitleEvent)events[0]).ResultCode == 0;
    }

    public async Task<BotUserInfo?> FetchUserInfo(uint uin, bool refreshCache = false)
    {
        return await Collection.Business.CachingLogic.GetCachedUsers(uin, refreshCache);
    }

    public async Task<bool> SetMessageReaction(uint groupUin, uint sequence, string code, bool isAdd)
    {
        if (isAdd)
        {
            var addReactionEvent = GroupAddReactionEvent.Create(groupUin, sequence, code);
            var results = await Collection.Business.SendEvent(addReactionEvent);
            return results.Count != 0 && results[0].ResultCode == 0;
        }
        else
        {
            var reduceReactionEvent = GroupReduceReactionEvent.Create(groupUin, sequence, code);
            var results = await Collection.Business.SendEvent(reduceReactionEvent);
            return results.Count != 0 && results[0].ResultCode == 0;
        }
    }

    public async Task<bool> SetNeedToConfirmSwitch(bool enableNoNeed)
    {
        var setNeedToConfirmSwitchEvent = SetNeedToConfirmSwitchEvent.Create(enableNoNeed);
        var results = await Collection.Business.SendEvent(setNeedToConfirmSwitchEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<List<string>?> FetchMarketFaceKey(List<string> faceIds)
    {
        var fetchMarketFaceKeyEvent = FetchMarketFaceKeyEvent.Create(faceIds);
        var results = await Collection.Business.SendEvent(fetchMarketFaceKeyEvent);
        return results.Count != 0 ? ((FetchMarketFaceKeyEvent)results[0]).Keys : null;
    }

    public async Task<BotGroupClockInResult> GroupClockIn(uint groupUin)
    {
        var groupClockInEvent = GroupClockInEvent.Create(groupUin);
        var results = await Collection.Business.SendEvent(groupClockInEvent);
        return ((GroupClockInEvent)results[0]).ResultInfo ?? new BotGroupClockInResult(false);
    }

    public Task<MessageResult> FriendSpecialShake(uint friendUin, SpecialPokeFaceType type, uint count)
    {
        var chain = MessageBuilder.Friend(friendUin)
            .SpecialPoke(type, count)
            .Build();
        return SendMessage(chain);
    }

    public Task<MessageResult> FriendShake(uint friendUin, PokeFaceType type, uint strength)
    {
        var chain = MessageBuilder.Friend(friendUin)
            .Poke(type, strength)
            .Build();
        return SendMessage(chain);
    }

    public async Task<bool> SetAvatar(ImageEntity avatar)
    {
        if (avatar.ImageStream == null) return false;

        var highwayUrlEvent = HighwayUrlEvent.Create();
        var highwayUrlResults = await Collection.Business.SendEvent(highwayUrlEvent);
        if (highwayUrlResults.Count == 0) return false;

        var ticket = ((HighwayUrlEvent)highwayUrlResults[0]).SigSession;
        var md5 = avatar.ImageStream.Value.Md5().UnHex();
        return await Collection.Highway.UploadSrcByStreamAsync(90, avatar.ImageStream.Value, ticket, md5,
            Array.Empty<byte>());
    }

    public async Task<bool> GroupSetAvatar(uint groupUin, ImageEntity avatar)
    {
        if (avatar.ImageStream == null) return false;

        var highwayUrlEvent = HighwayUrlEvent.Create();
        var highwayUrlResults = await Collection.Business.SendEvent(highwayUrlEvent);
        if (highwayUrlResults.Count == 0) return false;

        var ticket = ((HighwayUrlEvent)highwayUrlResults[0]).SigSession;
        var md5 = avatar.ImageStream.Value.Md5().UnHex();
        var extra = new GroupAvatarExtra
        {
            Type = 101,
            GroupUin = groupUin,
            Field3 = new GroupAvatarExtraField3 { Field1 = 1 },
            Field5 = 3,
            Field6 = 1
        }.Serialize().ToArray();
        return await Collection.Highway.UploadSrcByStreamAsync(3000, avatar.ImageStream.Value, ticket, md5, extra);
    }

    public async Task<(uint, uint)> GroupRemainAtAll(uint groupUin)
    {
        var groupRemainAtAllEvent = FetchGroupAtAllRemainEvent.Create(groupUin);
        var results = await Collection.Business.SendEvent(groupRemainAtAllEvent);
        if (results.Count == 0) return (0, 0);

        var ret = (FetchGroupAtAllRemainEvent)results[0];
        return (ret.RemainAtAllCountForUin, ret.RemainAtAllCountForGroup);
    }

    public async Task<bool> FetchSuperFaceId(uint id) =>
        await Collection.Business.CachingLogic.GetCachedIsSuperFaceId(id);

    public async Task<SysFaceEntry?> FetchFaceEntity(uint id) =>
        await Collection.Business.CachingLogic.GetCachedFaceEntity(id);

    public async Task<bool> GroupJoinEmojiChain(uint groupUin, uint emojiId, uint targetMessageSeq)
    {
        var groupJoinEmojiChainEvent = GroupJoinEmojiChainEvent.Create(targetMessageSeq, emojiId, groupUin);
        var results = await Collection.Business.SendEvent(groupJoinEmojiChainEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> FriendJoinEmojiChain(uint friendUin, uint emojiId, uint targetMessageSeq)
    {
        string? friendUid = await Collection.Business.CachingLogic.ResolveUid(null, friendUin);
        if (friendUid == null) return false;
        var friendJoinEmojiChainEvent = FriendJoinEmojiChainEvent.Create(targetMessageSeq, emojiId, friendUid);
        var results = await Collection.Business.SendEvent(friendJoinEmojiChainEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<(int Code, string ErrMsg, string? Url)> GetGroupGenerateAiRecordUrl(uint groupUin, string character, string text, uint chatType)
    {
        var (code, errMsg, record) = await GetGroupGenerateAiRecord(groupUin, character, text, chatType);
        if (code != 0)
            return (code, errMsg, null);

        var recordGroupDownloadEvent = RecordGroupDownloadEvent.Create(groupUin, record!.MsgInfo!);
        var @event = await Collection.Business.SendEvent(recordGroupDownloadEvent);
        if (@event.Count == 0) return (-1, "running event missing!", null);

        var finalResult = (RecordGroupDownloadEvent)@event[0];
        return finalResult.ResultCode == 0
            ? (finalResult.ResultCode, string.Empty, finalResult.AudioUrl)
            : (finalResult.ResultCode, "Failed to get group ai record", null);
    }

    public async Task<(int Code, string ErrMsg, RecordEntity? Record)> GetGroupGenerateAiRecord(uint groupUin, string character, string text, uint chatType)
    {
        var groupAiRecordEvent = GroupAiRecordEvent.Create(groupUin, character, text, chatType, (uint)Random.Shared.Next());
        while (true)
        {
            var results = await Collection.Business.SendEvent(groupAiRecordEvent);
            if (results.Count == 0) return (-1, "running event missing!", null);
            var aiRecordResult = (GroupAiRecordEvent)results[0];
            if (aiRecordResult.ResultCode != 0)
                return (aiRecordResult.ResultCode, aiRecordResult.ErrorMessage, null);
            if (aiRecordResult.RecordInfo is not null)
            {
                var index = aiRecordResult.RecordInfo!.MsgInfoBody[0].Index;
                return (aiRecordResult.ResultCode, string.Empty, new RecordEntity(index.FileUuid, index.Info.FileName, index.Info.FileHash.UnHex())
                {
                    AudioLength = (int)index.Info.Time,
                    FileSha1 = index.Info.FileSha1,
                    MsgInfo = aiRecordResult.RecordInfo
                });
            }
        }

    }

    public async Task<(int Code, string ErrMsg, List<AiCharacterList>? Result)> GetAiCharacters(uint chatType, uint groupUin)
    {
        var fetchAiRecordListEvent = FetchAiCharacterListEvent.Create(chatType, groupUin);

        var results = await Collection.Business.SendEvent(fetchAiRecordListEvent);
        if (results.Count == 0) return (-1, "Event missing!", null);

        var result = (FetchAiCharacterListEvent)results[0];

        return (result.ResultCode, result.ErrorMessage, result.AiCharacters);
    }

    public async Task<string> UploadImage(ImageEntity image)
    {
        await Collection.Highway.ManualUploadEntity(image);
        var msgInfo = image.MsgInfo;
        if (msgInfo is null) throw new Exception();
        var downloadEvent = ImageDownloadEvent.Create(Collection.Keystore.Uid ?? "", msgInfo);
        var result = await Collection.Business.SendEvent(downloadEvent);
        var ret = (ImageDownloadEvent)result[0];
        return ret.ImageUrl;
    }

    public async Task<ImageOcrResult?> ImageOcr(string imageUrl)
    {
        var imageOcrEvent = ImageOcrEvent.Create(imageUrl);
        var results = await Collection.Business.SendEvent(imageOcrEvent);
        return results.Count != 0 ? ((ImageOcrEvent)results[0]).ImageOcrResult : null;
    }

    public async Task<ImageOcrResult?> ImageOcr(ImageEntity image)
    {
        var imageUrl = await UploadImage(image);
        return await ImageOcr(imageUrl);
    }

    public async Task<(int Retcode, string Message, List<uint> FriendUins, List<uint> GroupUins)> GetPins()
    {
        var @event = FetchPinsEvent.Create();

        var results = await Collection.Business.SendEvent(@event);
        if (results.Count == 0)
        {
            return (-1, "No Result", new(), new());
        }

        var result = (FetchPinsEvent)results[0];
        return (result.ResultCode, result.Message, result.FriendUins, result.GroupUins);
    }

    public async Task<(int Retcode, string Message)> SetPinFriend(uint uin, bool isPin)
    {
        var @event = SetPinFriendEvent.Create(uin, isPin);

        var results = await Collection.Business.SendEvent(@event);
        if (results.Count == 0)
        {
            return (-1, "No Result");
        }

        var result = (SetPinFriendEvent)results[0];
        return (result.ResultCode, result.Message);
    }

    public async Task<(int Retcode, string Message)> SetPinGroup(uint uin, bool isPin)
    {
        var @event = SetPinGroupEvent.Create(uin, isPin);

        var results = await Collection.Business.SendEvent(@event);
        if (results.Count == 0)
        {
            return (-1, "No Result");
        }

        var result = (SetPinGroupEvent)results[0];
        return (result.ResultCode, result.Message);
    }
}