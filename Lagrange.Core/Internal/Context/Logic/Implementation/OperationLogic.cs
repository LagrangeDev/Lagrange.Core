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

    public async Task<List<string>> GetCookies(List<string> domains, CancellationToken cancellation)
    {
        var fetchCookieEvent = FetchCookieEvent.Create(domains);
        var events = await Collection.Business.SendEvent(fetchCookieEvent, cancellation);
        return events.Count != 0 ? ((FetchCookieEvent)events[0]).Cookies : new List<string>();
    }

    public Task<List<BotFriend>> FetchFriends(CancellationToken cancellationToken, bool refreshCache = false) =>
        Collection.Business.CachingLogic.GetCachedFriends(refreshCache, cancellationToken);

    public Task<List<BotGroupMember>> FetchMembers(uint groupUin, CancellationToken cancellationToken, bool refreshCache = false) =>
        Collection.Business.CachingLogic.GetCachedMembers(groupUin, refreshCache, cancellationToken);

    public Task<List<BotGroup>> FetchGroups(bool refreshCache, CancellationToken cancellationToken) =>
        Collection.Business.CachingLogic.GetCachedGroups(refreshCache, cancellationToken);

    public async Task<MessageResult> SendMessage(MessageChain chain, CancellationToken cancellation)
    {
        uint clientSeq = chain.ClientSequence;
        ulong messageId = chain.MessageId;

        var sendMessageEvent = SendMessageEvent.Create(chain);
        var events = await Collection.Business.SendEvent(sendMessageEvent, cancellation);
        if (events.Count == 0) return new MessageResult { Result = 9057 };

        var result = ((SendMessageEvent)events[0]).MsgResult;
        result.ClientSequence = clientSeq;
        result.MessageId = messageId;
        return result;
    }

    public async Task<bool> MuteGroupMember(uint groupUin, uint targetUin, uint duration, CancellationToken cancellationToken)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin, cancellationToken);
        if (uid == null) return false;

        var muteGroupMemberEvent = GroupMuteMemberEvent.Create(groupUin, duration, uid);
        var events = await Collection.Business.SendEvent(muteGroupMemberEvent, cancellationToken);
        return events.Count != 0 && ((GroupMuteMemberEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> MuteGroupGlobal(uint groupUin, bool isMute, CancellationToken cancellationToken)
    {
        var muteGroupMemberEvent = GroupMuteGlobalEvent.Create(groupUin, isMute);
        var events = await Collection.Business.SendEvent(muteGroupMemberEvent, cancellationToken);
        return events.Count != 0 && ((GroupMuteGlobalEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> KickGroupMember(uint groupUin, uint targetUin, bool rejectAddRequest, string reason, CancellationToken cancellationToken)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin, cancellationToken);
        if (uid == null) return false;

        var muteGroupMemberEvent = GroupKickMemberEvent.Create(groupUin, uid, rejectAddRequest, reason);
        var events = await Collection.Business.SendEvent(muteGroupMemberEvent, cancellationToken);
        return events.Count != 0 && ((GroupKickMemberEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> SetGroupAdmin(uint groupUin, uint targetUin, bool isAdmin, CancellationToken cancellationToken)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin, cancellationToken);
        if (uid == null) return false;

        var muteGroupMemberEvent = GroupSetAdminEvent.Create(groupUin, uid, isAdmin);
        var events = await Collection.Business.SendEvent(muteGroupMemberEvent, cancellationToken);
        return events.Count != 0 && ((GroupSetAdminEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> SetGroupBot(uint BotId, uint On, uint groupUin, CancellationToken cancellationToken)
    {
        var muteBotEvent = GroupSetBotEvent.Create(BotId, On, groupUin);
        var events = await Collection.Business.SendEvent(muteBotEvent, cancellationToken);
        return events.Count != 0 && ((GroupSetBotEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> SetGroupBotHD(uint BotId, uint groupUin, string? data_1, string? data_2, CancellationToken cancellationToken)
    {
        var muteBotEvent = GroupSetBothdEvent.Create(BotId, groupUin, data_1, data_2);
        var events = await Collection.Business.SendEvent(muteBotEvent, cancellationToken);
        return events.Count != 0 && ((GroupSetBothdEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RenameGroupMember(uint groupUin, uint targetUin, string targetName, CancellationToken cancellationToken)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin, cancellationToken);
        if (uid == null) return false;

        var renameGroupEvent = RenameMemberEvent.Create(groupUin, uid, targetName);
        var events = await Collection.Business.SendEvent(renameGroupEvent, cancellationToken);
        return events.Count != 0 && ((RenameMemberEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RenameGroup(uint groupUin, string targetName, CancellationToken cancellationToken)
    {
        var renameGroupEvent = GroupRenameEvent.Create(groupUin, targetName);
        var events = await Collection.Business.SendEvent(renameGroupEvent, cancellationToken);
        return events.Count != 0 && ((GroupRenameEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RemarkGroup(uint groupUin, string targetRemark, CancellationToken cancellationToken)
    {
        var renameGroupEvent = GroupRemarkEvent.Create(groupUin, targetRemark);
        var events = await Collection.Business.SendEvent(renameGroupEvent, cancellationToken);
        return events.Count != 0 && ((GroupRemarkEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> LeaveGroup(uint groupUin, CancellationToken cancellationToken)
    {
        var leaveGroupEvent = GroupLeaveEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(leaveGroupEvent, cancellationToken);
        return events.Count != 0 && ((GroupLeaveEvent)events[0]).ResultCode == 0;
    }

    public async Task<ulong> FetchGroupFSSpace(uint groupUin, CancellationToken cancellationToken)
    {
        var groupFSSpaceEvent = GroupFSSpaceEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(groupFSSpaceEvent, cancellationToken);
        return ((GroupFSSpaceEvent)events[0]).TotalSpace - ((GroupFSSpaceEvent)events[0]).UsedSpace;
    }

    public async Task<uint> FetchGroupFSCount(uint groupUin, CancellationToken cancellationToken)
    {
        var groupFSSpaceEvent = GroupFSCountEvent.Create(groupUin);
        var events = await Collection.Business.SendEvent(groupFSSpaceEvent, cancellationToken);
        return ((GroupFSCountEvent)events[0]).FileCount;
    }

    public async Task<List<IBotFSEntry>> FetchGroupFSList(uint groupUin, string targetDirectory, CancellationToken cancellationToken)
    {
        uint startIndex = 0;
        var entries = new List<IBotFSEntry>();
        while (true)
        {
            var groupFSListEvent = GroupFSListEvent.Create(groupUin, targetDirectory, startIndex, 20);
            var events = await Collection.Business.SendEvent(groupFSListEvent, cancellationToken);
            if (events.Count == 0) break;
            entries.AddRange(((GroupFSListEvent)events[0]).FileEntries);
            if (((GroupFSListEvent)events[0]).IsEnd) break;
            startIndex += 20;
        }
        return entries;
    }

    public async Task<string> FetchGroupFSDownload(uint groupUin, string fileId, CancellationToken cancellationToken)
    {
        var groupFSDownloadEvent = GroupFSDownloadEvent.Create(groupUin, fileId);
        var events = await Collection.Business.SendEvent(groupFSDownloadEvent, cancellationToken);
        return $"{((GroupFSDownloadEvent)events[0]).FileUrl}{fileId}";
    }

    public async Task<(int, string)> GroupFSMove(uint groupUin, string fileId, string parentDirectory, string targetDirectory, CancellationToken cancellationToken)
    {
        var groupFSMoveEvent = GroupFSMoveEvent.Create(groupUin, fileId, parentDirectory, targetDirectory);
        var events = await Collection.Business.SendEvent(groupFSMoveEvent, cancellationToken);
        int retCode = events.Count > 0 ? ((GroupFSMoveEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSMoveEvent)events[0]).RetMsg : string.Empty;
        return (retCode, retMsg);
    }

    public async Task<(int, string)> GroupFSDelete(uint groupUin, string fileId, CancellationToken cancellationToken)
    {
        var groupFSDeleteEvent = GroupFSDeleteEvent.Create(groupUin, fileId);
        var events = await Collection.Business.SendEvent(groupFSDeleteEvent, cancellationToken);
        int retCode = events.Count > 0 ? ((GroupFSDeleteEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSDeleteEvent)events[0]).RetMsg : string.Empty;
        return (retCode, retMsg);
    }

    public async Task<(int, string)> GroupFSCreateFolder(uint groupUin, string name, CancellationToken cancellationToken)
    {
        var groupFSCreateFolderEvent = GroupFSCreateFolderEvent.Create(groupUin, name);
        var events = await Collection.Business.SendEvent(groupFSCreateFolderEvent, cancellationToken);
        int retCode = events.Count > 0 ? ((GroupFSCreateFolderEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSCreateFolderEvent)events[0]).RetMsg : string.Empty;
        return (retCode, retMsg);
    }

    public async Task<(int, string)> GroupFSDeleteFolder(uint groupUin, string folderId, CancellationToken cancellationToken)
    {
        var groupFSDeleteFolderEvent = GroupFSDeleteFolderEvent.Create(groupUin, folderId);
        var events = await Collection.Business.SendEvent(groupFSDeleteFolderEvent, cancellationToken);
        int retCode = events.Count > 0 ? ((GroupFSDeleteFolderEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSDeleteFolderEvent)events[0]).RetMsg : string.Empty;
        return (retCode, retMsg);
    }

    public async Task<(int, string)> GroupFSRenameFolder(uint groupUin, string folderId, string newFolderName, CancellationToken cancellationToken)
    {
        var groupFSDeleteFolderEvent = GroupFSRenameFolderEvent.Create(groupUin, folderId, newFolderName);
        var events = await Collection.Business.SendEvent(groupFSDeleteFolderEvent, cancellationToken);
        int retCode = events.Count > 0 ? ((GroupFSRenameFolderEvent)events[0]).ResultCode : -1;
        string retMsg = events.Count > 0 ? ((GroupFSRenameFolderEvent)events[0]).RetMsg : "";
        return (retCode, retMsg);
    }

    public Task<bool> GroupFSUpload(uint groupUin, FileEntity fileEntity, string targetDirectory, CancellationToken cancellationToken)
    {
        try
        {
            return FileUploader.UploadGroup(Collection, MessageBuilder.Group(groupUin).Build(), fileEntity, targetDirectory, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public async Task<bool> UploadFriendFile(uint targetUin, FileEntity fileEntity, CancellationToken cancellationToken)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(null, targetUin, cancellationToken);
        var chain = new MessageChain(targetUin, Collection.Keystore.Uid ?? "", uid ?? "") { fileEntity };

        try
        {
            return await FileUploader.UploadPrivate(Collection, chain, fileEntity, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RecallGroupMessage(uint groupUin, MessageResult result, CancellationToken cancellationToken)
    {
        if (result.Sequence == null) return false;

        var recallMessageEvent = RecallGroupMessageEvent.Create(groupUin, result.Sequence.Value);
        var events = await Collection.Business.SendEvent(recallMessageEvent, cancellationToken);
        return events.Count != 0 && ((RecallGroupMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RecallGroupMessage(MessageChain chain, CancellationToken cancellationToken)
    {
        if (chain.GroupUin == null) return false;

        var recallMessageEvent = RecallGroupMessageEvent.Create(chain.GroupUin.Value, chain.Sequence);
        var events = await Collection.Business.SendEvent(recallMessageEvent, cancellationToken);
        return events.Count != 0 && ((RecallGroupMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RecallGroupMessage(uint groupUin, uint sequence, CancellationToken cancellationToken)
    {
        var recallMessageEvent = RecallGroupMessageEvent.Create(groupUin, sequence);
        var events = await Collection.Business.SendEvent(recallMessageEvent, cancellationToken);
        return events.Count != 0 && ((RecallGroupMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RecallFriendMessage(uint friendUin, MessageResult result, CancellationToken cancellationToken)
    {
        if (result.Sequence == null) return false;
        if (await Collection.Business.CachingLogic.ResolveUid(null, friendUin, cancellationToken) is not { } uid) return false;

        var recallMessageEvent = RecallFriendMessageEvent.Create(uid, result.ClientSequence, result.Sequence ?? 0, (uint)(result.MessageId & uint.MaxValue), result.Timestamp);
        var events = await Collection.Business.SendEvent(recallMessageEvent, cancellationToken);
        return events.Count != 0 && ((RecallFriendMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> RecallFriendMessage(MessageChain chain, CancellationToken cancellationToken)
    {
        if (await Collection.Business.CachingLogic.ResolveUid(null, chain.TargetUin, cancellationToken) is not { } uid) return false;

        uint timestamp = (uint)new DateTimeOffset(chain.Time).ToUnixTimeSeconds();
        var recallMessageEvent = RecallFriendMessageEvent.Create(uid, chain.ClientSequence, chain.Sequence, (uint)(chain.MessageId & uint.MaxValue), timestamp);
        var events = await Collection.Business.SendEvent(recallMessageEvent, cancellationToken);
        return events.Count != 0 && ((RecallFriendMessageEvent)events[0]).ResultCode == 0;
    }

    public async Task<List<BotGroupRequest>?> FetchGroupRequests(CancellationToken cancellationToken)
    {
        var fetchRequestsEvent = FetchGroupRequestsEvent.Create();
        var events = await Collection.Business.SendEvent(fetchRequestsEvent, cancellationToken);
        if (events.Count == 0) return null;

        var resolved = events.Cast<FetchGroupRequestsEvent>().SelectMany(e => e.Events).ToList();
        var results = new List<BotGroupRequest>();

        foreach (var result in resolved)
        {
            var uins = await Task.WhenAll(ResolveUid(result.InvitorMemberUid), ResolveUid(result.TargetMemberUid), ResolveUid(result.OperatorUid));
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
            var e = await Collection.Business.SendEvent(fetchUidEvent, cancellationToken);
            return e.Count == 0 ? 0 : ((FetchUserInfoEvent)e[0]).UserInfo.Uin;
        }
    }

    public async Task<List<BotFriendRequest>?> FetchFriendRequests(CancellationToken cancellationToken)
    {
        var fetchRequestsEvent = FetchFriendsRequestsEvent.Create();
        var events = await Collection.Business.SendEvent(fetchRequestsEvent, cancellationToken);
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
            var e = await Collection.Business.SendEvent(fetchUidEvent, cancellationToken);
            return e.Count == 0 ? 0 : ((FetchUserInfoEvent)e[0]).UserInfo.Uin;
        }
    }

    public async Task<bool> GroupTransfer(uint groupUin, uint targetUin, CancellationToken cancellationToken)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin, cancellationToken);
        if (uid == null || Collection.Keystore.Uid is not { } source) return false;

        var transferEvent = GroupTransferEvent.Create(groupUin, source, uid);
        var results = await Collection.Business.SendEvent(transferEvent, cancellationToken);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> SetStatus(uint status, CancellationToken cancellationToken)
    {
        var setStatusEvent = SetStatusEvent.Create(status, 0);
        var results = await Collection.Business.SendEvent(setStatusEvent, cancellationToken);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> SetCustomStatus(uint faceId, string text, CancellationToken cancellationToken)
    {
        var setCustomStatusEvent = SetCustomStatusEvent.Create(faceId, text);
        var results = await Collection.Business.SendEvent(setCustomStatusEvent, cancellationToken);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> RequestFriend(uint targetUin, string question, string message, CancellationToken cancellationToken)
    {
        var requestFriendSearchEvent = RequestFriendSearchEvent.Create(targetUin);
        var searchEvents = await Collection.Business.SendEvent(requestFriendSearchEvent, cancellationToken);
        if (searchEvents.Count == 0) return false;
        await Task.Delay(5000);

        var requestFriendSettingEvent = RequestFriendSettingEvent.Create(targetUin);
        var settingEvents = await Collection.Business.SendEvent(requestFriendSettingEvent, cancellationToken);
        if (settingEvents.Count == 0) return false;

        var requestFriendEvent = RequestFriendEvent.Create(targetUin, message, question);
        var events = await Collection.Business.SendEvent(requestFriendEvent, cancellationToken);
        return events.Count != 0 && ((RequestFriendEvent)events[0]).ResultCode == 0;
    }

    public async Task<bool> Like(uint targetUin, uint count, CancellationToken cancellationToken)
    {
        var uid = await Collection.Business.CachingLogic.ResolveUid(null, targetUin, cancellationToken);
        if (uid == null) return false;

        var friendLikeEvent = FriendLikeEvent.Create(uid, count);
        var results = await Collection.Business.SendEvent(friendLikeEvent, cancellationToken);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> InviteGroup(uint targetGroupUin, Dictionary<uint, uint?> invitedUins, CancellationToken cancellationToken)
    {
        var invitedUids = new Dictionary<string, uint?>(invitedUins.Count);
        foreach (var (friendUin, groupUin) in invitedUins)
        {
            string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, friendUin, cancellationToken);
            if (uid != null) invitedUids[uid] = groupUin;
        }

        var @event = GroupInviteEvent.Create(targetGroupUin, invitedUids);
        var results = await Collection.Business.SendEvent(@event, cancellationToken);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<string?> GetClientKey(CancellationToken cancellationToken)
    {
        var clientKeyEvent = FetchClientKeyEvent.Create();
        var events = await Collection.Business.SendEvent(clientKeyEvent, cancellationToken);
        return events.Count == 0 ? null : ((FetchClientKeyEvent)events[0]).ClientKey;
    }

    public async Task<bool> SetGroupRequest(uint groupUin, ulong sequence, uint type, bool accept, string reason, CancellationToken cancellationToken)
    {
        var inviteEvent = SetGroupRequestEvent.Create(accept, groupUin, sequence, type, reason);
        var results = await Collection.Business.SendEvent(inviteEvent, cancellationToken);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> SetGroupFilteredRequest(uint groupUin, ulong sequence, uint type, bool accept, string reason, CancellationToken cancellationToken)
    {
        var inviteEvent = SetGroupFilteredRequestEvent.Create(accept, groupUin, sequence, type, reason);
        var results = await Collection.Business.SendEvent(inviteEvent, cancellationToken);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<bool> SetFriendRequest(string targetUid, bool accept, CancellationToken cancellationToken)
    {
        var inviteEvent = SetFriendRequestEvent.Create(targetUid, accept);
        var results = await Collection.Business.SendEvent(inviteEvent, cancellationToken);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<List<MessageChain>?> GetGroupMessage(uint groupUin, uint startSequence, uint endSequence, CancellationToken cancellationToken)
    {
        var getMsgEvent = GetGroupMessageEvent.Create(groupUin, startSequence, endSequence);
        var results = await Collection.Business.SendEvent(getMsgEvent, cancellationToken);
        return results.Count != 0 ? ((GetGroupMessageEvent)results[0]).Chains : null;
    }

    public async Task<List<MessageChain>?> GetRoamMessage(uint friendUin, uint time, uint count, CancellationToken cancellationToken)
    {
        if (await Collection.Business.CachingLogic.ResolveUid(null, friendUin, cancellationToken) is not { } uid) return null;

        var roamEvent = GetRoamMessageEvent.Create(uid, time, count);
        var results = await Collection.Business.SendEvent(roamEvent, cancellationToken);
        return results.Count != 0 ? ((GetRoamMessageEvent)results[0]).Chains : null;
    }

    public async Task<List<MessageChain>?> GetC2cMessage(uint friendUin, uint startSequence, uint endSequence, CancellationToken cancellationToken)
    {
        if (await Collection.Business.CachingLogic.ResolveUid(null, friendUin, cancellationToken) is not { } uid) return null;

        var c2cEvent = GetC2cMessageEvent.Create(uid, startSequence, endSequence);
        var results = await Collection.Business.SendEvent(c2cEvent, cancellationToken);
        return results.Count != 0 ? ((GetC2cMessageEvent)results[0]).Chains : null;
    }

    public async Task<(int code, List<MessageChain>? chains)> GetMessagesByResId(string resId, CancellationToken cancellationToken)
    {
        var @event = MultiMsgDownloadEvent.Create(Collection.Keystore.Uid ?? "", resId);
        var results = await Collection.Business.SendEvent(@event, cancellationToken);

        if (results.Count == 0) return (-9999, null);
        var result = (MultiMsgDownloadEvent)results[0];

        return (result.ResultCode, result.Chains);
    }

    public async Task<List<string>?> FetchCustomFace(CancellationToken cancellationToken)
    {
        var fetchCustomFaceEvent = FetchCustomFaceEvent.Create();
        var results = await Collection.Business.SendEvent(fetchCustomFaceEvent, cancellationToken);
        return results.Count != 0 ? ((FetchCustomFaceEvent)results[0]).Urls : null;
    }

    public async Task<string?> UploadLongMessage(List<MessageChain> chains, CancellationToken cancellationToken)
    {
        var multiMsgUploadEvent = MultiMsgUploadEvent.Create(null, chains);
        var results = await Collection.Business.SendEvent(multiMsgUploadEvent, cancellationToken);
        return results.Count != 0 ? ((MultiMsgUploadEvent)results[0]).ResId : null;
    }

    public async Task<bool> MarkAsRead(uint groupUin, string? targetUid, uint startSequence, uint time, CancellationToken cancellationToken)
    {
        var markAsReadEvent = MarkReadedEvent.Create(groupUin, targetUid, startSequence, time);
        var results = await Collection.Business.SendEvent(markAsReadEvent, cancellationToken);
        return results.Count != 0 && ((MarkReadedEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> FriendPoke(uint friendUin, CancellationToken cancellationToken)
    {
        var friendPokeEvent = FriendPokeEvent.Create(friendUin);
        var results = await Collection.Business.SendEvent(friendPokeEvent, cancellationToken);
        return results.Count != 0 && ((FriendPokeEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> GroupPoke(uint groupUin, uint friendUin, CancellationToken cancellationToken)
    {
        var friendPokeEvent = GroupPokeEvent.Create(friendUin, groupUin);
        var results = await Collection.Business.SendEvent(friendPokeEvent, cancellationToken);
        return results.Count != 0 && ((FriendPokeEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> SetEssenceMessage(uint groupUin, uint sequence, uint random, CancellationToken cancellationToken)
    {
        var setEssenceMessageEvent = SetEssenceMessageEvent.Create(groupUin, sequence, random);
        var results = await Collection.Business.SendEvent(setEssenceMessageEvent, cancellationToken);
        return results.Count != 0 && ((SetEssenceMessageEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> RemoveEssenceMessage(uint groupUin, uint sequence, uint random, CancellationToken cancellationToken)
    {
        var removeEssenceMessageEvent = RemoveEssenceMessageEvent.Create(groupUin, sequence, random);
        var results = await Collection.Business.SendEvent(removeEssenceMessageEvent, cancellationToken);
        return results.Count != 0 && ((RemoveEssenceMessageEvent)results[0]).ResultCode == 0;
    }

    public async Task<bool> GroupSetSpecialTitle(uint groupUin, uint targetUin, string title, CancellationToken cancellationToken)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin, cancellationToken);
        if (uid == null) return false;

        var groupSetSpecialTitleEvent = GroupSetSpecialTitleEvent.Create(groupUin, uid, title);
        var events = await Collection.Business.SendEvent(groupSetSpecialTitleEvent, cancellationToken);
        return events.Count != 0 && ((GroupSetSpecialTitleEvent)events[0]).ResultCode == 0;
    }

    public async Task<BotUserInfo?> FetchUserInfo(uint uin, CancellationToken cancellationToken, bool refreshCache = false)
    {
        return await Collection.Business.CachingLogic.GetCachedUsers(uin, refreshCache, cancellationToken);
    }

    public async Task<bool> SetMessageReaction(uint groupUin, uint sequence, string code, bool isAdd, CancellationToken cancellationToken)
    {
        if (isAdd)
        {
            var addReactionEvent = GroupAddReactionEvent.Create(groupUin, sequence, code);
            var results = await Collection.Business.SendEvent(addReactionEvent, cancellationToken);
            return results.Count != 0 && results[0].ResultCode == 0;
        }
        else
        {
            var reduceReactionEvent = GroupReduceReactionEvent.Create(groupUin, sequence, code);
            var results = await Collection.Business.SendEvent(reduceReactionEvent, cancellationToken);
            return results.Count != 0 && results[0].ResultCode == 0;
        }
    }

    public async Task<bool> SetNeedToConfirmSwitch(bool enableNoNeed, CancellationToken cancellationToken)
    {
        var setNeedToConfirmSwitchEvent = SetNeedToConfirmSwitchEvent.Create(enableNoNeed);
        var results = await Collection.Business.SendEvent(setNeedToConfirmSwitchEvent, cancellationToken);
        return results.Count != 0 && results[0].ResultCode == 0;
    }

    public async Task<List<string>?> FetchMarketFaceKey(List<string> faceIds, CancellationToken cancellationToken)
    {
        var fetchMarketFaceKeyEvent = FetchMarketFaceKeyEvent.Create(faceIds);
        var results = await Collection.Business.SendEvent(fetchMarketFaceKeyEvent, cancellationToken);
        return results.Count != 0 ? ((FetchMarketFaceKeyEvent)results[0]).Keys : null;
    }

    public async Task<BotGroupClockInResult> GroupClockIn(uint groupUin, CancellationToken cancellationToken)
    {
        var groupClockInEvent = GroupClockInEvent.Create(groupUin);
        var results = await Collection.Business.SendEvent(groupClockInEvent, cancellationToken);
        return ((GroupClockInEvent)results[0]).ResultInfo ?? new BotGroupClockInResult(false);
    }

    public Task<MessageResult> FriendSpecialShake(uint friendUin, SpecialPokeFaceType type, uint count, CancellationToken cancellationToken)
    {
        var chain = MessageBuilder.Friend(friendUin)
            .SpecialPoke(type, count)
            .Build();
        return SendMessage(chain, cancellationToken);
    }

    public Task<MessageResult> FriendShake(uint friendUin, PokeFaceType type, uint strength, CancellationToken cancellationToken)
    {
        var chain = MessageBuilder.Friend(friendUin)
            .Poke(type, strength)
            .Build();
        return SendMessage(chain, cancellationToken);
    }

    public async Task<bool> SetAvatar(ImageEntity avatar, CancellationToken cancellationToken)
    {
        if (avatar.ImageStream == null) return false;
        
        var highwayUrlEvent = HighwayUrlEvent.Create();
        var highwayUrlResults = await Collection.Business.SendEvent(highwayUrlEvent, cancellationToken);
        if (highwayUrlResults.Count == 0) return false;
        
        var ticket = ((HighwayUrlEvent)highwayUrlResults[0]).SigSession;
        var md5 = avatar.ImageStream.Value.Md5().UnHex();
        return await Collection.Highway.UploadSrcByStreamAsync(90, avatar.ImageStream.Value, ticket, md5, cancellationToken, extendInfo: Array.Empty<byte>());
    }
    
    public async Task<bool> GroupSetAvatar(uint groupUin, ImageEntity avatar, CancellationToken cancellationToken)
    {
        if (avatar.ImageStream == null) return false;
        
        var highwayUrlEvent = HighwayUrlEvent.Create();
        var highwayUrlResults = await Collection.Business.SendEvent(highwayUrlEvent, cancellationToken);
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
        return await Collection.Highway.UploadSrcByStreamAsync(3000, avatar.ImageStream.Value, ticket, md5, cancellationToken, extendInfo: extra);
    }

    public async Task<(uint, uint)> GroupRemainAtAll(uint groupUin, CancellationToken cancellationToken)
    {
        var groupRemainAtAllEvent = FetchGroupAtAllRemainEvent.Create(groupUin);
        var results = await Collection.Business.SendEvent(groupRemainAtAllEvent, cancellationToken);
        if (results.Count == 0) return (0, 0);

        var ret = (FetchGroupAtAllRemainEvent)results[0];
        return (ret.RemainAtAllCountForUin, ret.RemainAtAllCountForGroup);
    }
}
