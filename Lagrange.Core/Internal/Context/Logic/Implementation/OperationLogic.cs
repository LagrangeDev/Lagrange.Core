using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Context.Attributes;
using Lagrange.Core.Internal.Context.Uploader;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

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
        var sendMessageEvent = SendMessageEvent.Create(chain);
        var events = await Collection.Business.SendEvent(sendMessageEvent);
        return ((SendMessageEvent)events[0]).MsgResult;
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
    
    public async Task<bool> KickGroupMember(uint groupUin, uint targetUin, bool rejectAddRequest)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin);
        if (uid == null) return false;
        
        var muteGroupMemberEvent = GroupKickMemberEvent.Create(groupUin, uid, rejectAddRequest);
        var events = await Collection.Business.SendEvent(muteGroupMemberEvent);
        return events.Count != 0 && ((GroupKickMemberEvent)events[0]).ResultCode == 0;
    }
    
    public async Task<bool> SetGroupAdmin(uint groupUin, uint targetUin, bool isAdmin)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(groupUin, targetUin);
        if (uid == null) return false;
        
        var muteGroupMemberEvent = GroupSetAdminEvent.Create(groupUin, uid, isAdmin);
        var events = await Collection.Business.SendEvent(muteGroupMemberEvent);
        return events.Count != 0 && ((GroupSetAdminEvent)events[0]).ResultCode == 0;
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
    
    public async Task<List<IBotFSEntry>> FetchGroupFSList(uint groupUin, string targetDirectory, uint startIndex)
    {
        var groupFSListEvent = GroupFSListEvent.Create(groupUin, targetDirectory, startIndex);
        var events = await Collection.Business.SendEvent(groupFSListEvent);
        return ((GroupFSListEvent)events[0]).FileEntries;
    }

    public async Task<string> FetchGroupFSDownload(uint groupUin, string fileId)
    {
        var groupFSDownloadEvent = GroupFSDownloadEvent.Create(groupUin, fileId);
        var events = await Collection.Business.SendEvent(groupFSDownloadEvent);
        return $"{((GroupFSDownloadEvent)events[0]).FileUrl}{fileId}";
    }

    public async Task<bool> GroupFSMove(uint groupUin, string fileId, string parentDirectory, string targetDirectory)
    {
        var groupFSMoveEvent = GroupFSMoveEvent.Create(groupUin, fileId, parentDirectory, targetDirectory);
        var events = await Collection.Business.SendEvent(groupFSMoveEvent); 
        return events.Count != 0 && ((GroupFSMoveEvent)events[0]).ResultCode == 0;
    }
    
    public async Task<bool> GroupFSDelete(uint groupUin, string fileId)
    {
        var groupFSDeleteEvent = GroupFSDeleteEvent.Create(groupUin, fileId);
        var events = await Collection.Business.SendEvent(groupFSDeleteEvent); 
        return events.Count != 0 && ((GroupFSDeleteEvent)events[0]).ResultCode == 0;
    }
    
    public async Task<bool> GroupFSCreateFolder(uint groupUin, string name)
    {
        var groupFSCreateFolderEvent = GroupFSCreateFolderEvent.Create(groupUin, name);
        var events = await Collection.Business.SendEvent(groupFSCreateFolderEvent); 
        return events.Count != 0 && ((GroupFSCreateFolderEvent)events[0]).ResultCode == 0;
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

    public async Task<List<BotGroupRequest>?> FetchGroupRequests()
    {
        var fetchRequestsEvent = FetchGroupRequestsEvent.Create();
        var events = await Collection.Business.SendEvent(fetchRequestsEvent);
        if (events.Count == 0) return null;

        var resolved = ((FetchGroupRequestsEvent)events[0]).Events;
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
                result.Comment));
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

    public async Task<List<dynamic>?> FetchFriendRequests()
    {
        var fetchRequestsEvent = FetchFriendRequestsEvent.Create();
        var events = await Collection.Business.SendEvent(fetchRequestsEvent);
        if (events.Count == 0) return null;

        return null;
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

    public async Task<bool> SetGroupRequest(uint groupUin, ulong sequence, uint type, bool accept)
    {
        var inviteEvent = SetGroupRequestEvent.Create(accept, groupUin, sequence, type);
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

    public async Task<BotUserInfo?> FetchUserInfo(uint uin)
    {
        string? uid = await Collection.Business.CachingLogic.ResolveUid(null, uin);
        if (uid == null) return null;
        
        var fetchUserInfoEvent = FetchUserInfoEvent.Create(uid);
        var events = await Collection.Business.SendEvent(fetchUserInfoEvent);
        return events.Count != 0 ? ((FetchUserInfoEvent)events[0]).UserInfo : null;
    }
    
    public async Task<bool> SetMessageReaction(uint groupUin, uint sequence, string code)
    {
        var setReactionEvent = GroupSetReactionEvent.Create(groupUin, sequence, code);
        var results = await Collection.Business.SendEvent(setReactionEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }
    
    public async Task<bool> SetNeedToConfirmSwitch(bool enableNoNeed)
    {
        var setNeedToConfirmSwitchEvent = SetNeedToConfirmSwitchEvent.Create(enableNoNeed);
        var results = await Collection.Business.SendEvent(setNeedToConfirmSwitchEvent);
        return results.Count != 0 && results[0].ResultCode == 0;
    }
}
