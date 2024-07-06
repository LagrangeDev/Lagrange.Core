using Lagrange.Core.Event;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Internal.Context.Attributes;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.Notify;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Service;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using FriendPokeEvent = Lagrange.Core.Event.EventArg.FriendPokeEvent;

namespace Lagrange.Core.Internal.Context.Logic.Implementation;

[EventSubscribe(typeof(PushMessageEvent))]
[EventSubscribe(typeof(SendMessageEvent))]
[EventSubscribe(typeof(MultiMsgUploadEvent))]
[EventSubscribe(typeof(GetRoamMessageEvent))]
[EventSubscribe(typeof(GetGroupMessageEvent))]
[EventSubscribe(typeof(GroupSysInviteEvent))]
[EventSubscribe(typeof(GroupSysAdminEvent))]
[EventSubscribe(typeof(GroupSysIncreaseEvent))]
[EventSubscribe(typeof(GroupSysDecreaseEvent))]
[EventSubscribe(typeof(GroupSysMuteEvent))]
[EventSubscribe(typeof(GroupSysMemberMuteEvent))]
[EventSubscribe(typeof(GroupSysRecallEvent))]
[EventSubscribe(typeof(GroupSysRequestJoinEvent))]
[EventSubscribe(typeof(GroupSysRequestInvitationEvent))]
[EventSubscribe(typeof(FriendSysRecallEvent))]
[EventSubscribe(typeof(FriendSysRequestEvent))]
[EventSubscribe(typeof(FriendSysPokeEvent))]
[EventSubscribe(typeof(LoginNotifyEvent))]
[BusinessLogic("MessagingLogic", "Manage the receiving and sending of messages and notifications")]
internal class MessagingLogic : LogicBase
{
    private const string Tag = nameof(MessagingLogic);

    internal MessagingLogic(ContextCollection collection) : base(collection) { }

    public override async Task Incoming(ProtocolEvent e)
    {
        switch (e)
        {
            case PushMessageEvent push:
            {
                if (push.Chain.Count == 0) return;
                await ResolveIncomingChain(push.Chain);
                await ResolveChainMetadata(push.Chain);

                var chain = push.Chain;
                Collection.Log.LogVerbose(Tag, chain.ToPreviewString());

                EventBase args = push.Chain.Type switch
                {
                    MessageChain.MessageType.Friend => new FriendMessageEvent(chain),
                    MessageChain.MessageType.Group => new GroupMessageEvent(chain),
                    MessageChain.MessageType.Temp => new TempMessageEvent(chain),
                    _ => throw new ArgumentOutOfRangeException()
                };
                Collection.Invoker.PostEvent(args);

                break;
            }
            case GetRoamMessageEvent get:
            {
                foreach (var chain in get.Chains)
                {
                    if (chain.Count == 0) return;
                    await ResolveIncomingChain(chain);
                    await ResolveChainMetadata(chain);
                }
                break;
            }
            case GetGroupMessageEvent get:
            {
                foreach (var chain in get.Chains)
                {
                    if (chain.Count == 0) return;
                    await ResolveIncomingChain(chain);
                    await ResolveChainMetadata(chain);
                }
                break;
            }
            case GroupSysInviteEvent invite:
            {
                uint invitorUin = await Collection.Business.CachingLogic.ResolveUin(null, invite.InvitorUid) ?? 0;
                var inviteArgs = new GroupInvitationEvent(invite.GroupUin, invitorUin);
                Collection.Invoker.PostEvent(inviteArgs);
                break;
            }
            case GroupSysAdminEvent admin:
            {
                uint adminUin = await Collection.Business.CachingLogic.ResolveUin(admin.GroupUin, admin.Uid) ?? 0;
                var adminArgs = new GroupAdminChangedEvent(admin.GroupUin, adminUin, admin.IsPromoted);
                Collection.Invoker.PostEvent(adminArgs);
                break;
            }
            case GroupSysIncreaseEvent increase:
            {
                uint memberUin = await Collection.Business.CachingLogic.ResolveUin(increase.GroupUin, increase.MemberUid, true) ?? 0;
                uint? invitorUin = null;
                if (increase.InvitorUid != null) invitorUin = await Collection.Business.CachingLogic.ResolveUin(increase.GroupUin, increase.InvitorUid);
                var increaseArgs = new GroupMemberIncreaseEvent(increase.GroupUin, memberUin, invitorUin, increase.Type);
                Collection.Invoker.PostEvent(increaseArgs);
                break;
            }
            case GroupSysDecreaseEvent decrease:
            {
                uint memberUin = await Collection.Business.CachingLogic.ResolveUin(decrease.GroupUin, decrease.MemberUid) ?? 0;
                uint? operatorUin = null;
                if (decrease.OperatorUid != null) operatorUin = await Collection.Business.CachingLogic.ResolveUin(decrease.GroupUin, decrease.OperatorUid);
                var decreaseArgs = new GroupMemberDecreaseEvent(decrease.GroupUin, memberUin, operatorUin, decrease.Type);
                Collection.Invoker.PostEvent(decreaseArgs);
                break;
            }
            case FriendSysRequestEvent info:
            {
                var requestArgs = new FriendRequestEvent(info.SourceUin, info.SourceUid, info.Message, info.Source);
                Collection.Invoker.PostEvent(requestArgs);
                break;
            }
            case GroupSysMuteEvent groupMute:
            {
                uint? operatorUin = null;
                if (groupMute.OperatorUid != null) operatorUin = await Collection.Business.CachingLogic.ResolveUin(groupMute.GroupUin, groupMute.OperatorUid);
                var muteArgs = new GroupMuteEvent(groupMute.GroupUin, operatorUin, groupMute.IsMuted);
                Collection.Invoker.PostEvent(muteArgs);
                break;
            }
            case GroupSysMemberMuteEvent memberMute:
            {
                uint memberUin = await Collection.Business.CachingLogic.ResolveUin(memberMute.GroupUin, memberMute.TargetUid) ?? 0;
                uint? operatorUin = null;
                if (memberMute.OperatorUid != null) operatorUin = await Collection.Business.CachingLogic.ResolveUin(memberMute.GroupUin, memberMute.OperatorUid);
                var muteArgs = new GroupMemberMuteEvent(memberMute.GroupUin, memberUin, operatorUin, memberMute.Duration);
                Collection.Invoker.PostEvent(muteArgs);
                break;
            }
            case GroupSysRecallEvent recall:
            {
                uint authorUin = await Collection.Business.CachingLogic.ResolveUin(recall.GroupUin, recall.AuthorUid) ?? 0;
                uint operatorUin = 0;
                if (recall.OperatorUid != null) operatorUin = await Collection.Business.CachingLogic.ResolveUin(recall.GroupUin, recall.OperatorUid) ?? 0;
                var recallArgs = new GroupRecallEvent(recall.GroupUin, authorUin, operatorUin, recall.Sequence, recall.Time, recall.Random);
                Collection.Invoker.PostEvent(recallArgs);
                break;
            }
            case GroupSysRequestJoinEvent join:
            {
                var fetchUidEvent = FetchUserInfoEvent.Create(join.TargetUid);
                var results = await Collection.Business.SendEvent(fetchUidEvent);
                uint targetUin = results.Count == 0 ? 0 : ((FetchUserInfoEvent)results[0]).UserInfo.Uin;
                
                var joinArgs = new GroupJoinRequestEvent(join.GroupUin, targetUin);
                Collection.Invoker.PostEvent(joinArgs);
                break;
            }
            case GroupSysRequestInvitationEvent invitation:
            {
                uint invitorUin = await Collection.Business.CachingLogic.ResolveUin(invitation.GroupUin, invitation.InvitorUid) ?? 0;
                
                var fetchUidEvent = FetchUserInfoEvent.Create(invitation.TargetUid);
                var results = await Collection.Business.SendEvent(fetchUidEvent);
                uint targetUin = results.Count == 0 ? 0 : ((FetchUserInfoEvent)results[0]).UserInfo.Uin;
                
                var invitationArgs = new GroupInvitationRequestEvent(invitation.GroupUin, targetUin, invitorUin);
                Collection.Invoker.PostEvent(invitationArgs);
                break;
            }
            case FriendSysRecallEvent recall:
            {
                uint fromUin = await Collection.Business.CachingLogic.ResolveUin(null, recall.FromUid) ?? 0;
                var recallArgs = new FriendRecallEvent(fromUin, recall.Sequence, recall.Time, recall.Random);
                Collection.Invoker.PostEvent(recallArgs);
                break;
            }
            case FriendSysPokeEvent poke:
            {
                var pokeArgs = new FriendPokeEvent(poke.FriendUin);
                Collection.Invoker.PostEvent(pokeArgs);
                break;
            }
            case LoginNotifyEvent login:
            {
                var deviceArgs = new DeviceLoginEvent(login.IsLogin, login.AppId, login.Tag, login.Message);
                Collection.Invoker.PostEvent(deviceArgs);
                break;
            }
        }
    }

    public override async Task Outgoing(ProtocolEvent e)
    {
        switch (e)
        {
            case MultiMsgUploadEvent { Chains: { } chains }:
            {
                foreach (var chain in chains)
                {
                    await ResolveChainMetadata(chain);
                    await ResolveOutgoingChain(chain);
                    await Collection.Highway.UploadResources(chain);
                }
                break;
            }
            case SendMessageEvent send: // resolve Uin to Uid
            {
                await ResolveChainMetadata(send.Chain);
                await ResolveOutgoingChain(send.Chain);
                await Collection.Highway.UploadResources(send.Chain);
                break;
            }
        }
    }

    private async Task ResolveIncomingChain(MessageChain chain)
    {
        foreach (var entity in chain) switch (entity)
        {
            case FileEntity { FileHash: not null, FileUuid: not null } file:  // private
            {
                var @event = FileDownloadEvent.Create(file.FileUuid, file.FileHash, chain.Uid, chain.SelfUid);
                var results = await Collection.Business.SendEvent(@event);
                if (results.Count != 0)
                {
                    var result = (FileDownloadEvent)results[0];
                    file.FileUrl = result.FileUrl;
                }
                
                break;
            }
            case FileEntity { FileId: not null } file when chain.GroupUin is not null:  // group
            {
                var @event = GroupFSDownloadEvent.Create(chain.GroupUin.Value, file.FileId);
                var results = await Collection.Business.SendEvent(@event);
                if (results.Count != 0)
                {
                    var result = (GroupFSDownloadEvent)results[0];
                    file.FileUrl = result.FileUrl;
                }
                
                break;
            }
            case MultiMsgEntity { ResId: not null } multi:
            {
                var @event = MultiMsgDownloadEvent.Create(chain.Uid ?? "", multi.ResId);
                var results = await Collection.Business.SendEvent(@event);
                if (results.Count != 0)
                {
                    var result = (MultiMsgDownloadEvent)results[0];
                    multi.Chains.AddRange((IEnumerable<MessageChain>?)result.Chains ?? Array.Empty<MessageChain>());
                }
                
                break;
            }
            case RecordEntity { MsgInfo: not null } record:
            {
                var @event = chain.IsGroup 
                    ? RecordGroupDownloadEvent.Create(chain.GroupUin ?? 0, record.MsgInfo) 
                    : RecordDownloadEvent.Create(chain.Uid ?? string.Empty, record.MsgInfo);
            
                var results = await Collection.Business.SendEvent(@event);
                if (results.Count != 0)
                {
                    var result = (RecordDownloadEvent)results[0];
                    record.AudioUrl = result.AudioUrl;
                }
                
                break;
            }
            case RecordEntity { AudioUuid: not null } record:
            {
                var @event = chain.IsGroup 
                    ? RecordGroupDownloadEvent.Create(chain.GroupUin ?? 0, record.AudioUuid) 
                    : RecordDownloadEvent.Create(chain.Uid ?? string.Empty, record.AudioUuid);
            
                var results = await Collection.Business.SendEvent(@event);
                if (results.Count != 0)
                {
                    var result = (RecordDownloadEvent)results[0];
                    record.AudioUrl = result.AudioUrl;
                }
                
                break;
            }
            case VideoEntity { VideoUuid: not null } video:
            {
                string uid = (chain.IsGroup
                    ? await Collection.Business.CachingLogic.ResolveUid(chain.GroupUin, chain.FriendUin)
                    : chain.Uid) ?? "";
                var @event = VideoDownloadEvent.Create(video.VideoUuid, uid, video.FilePath, "", "",chain.IsGroup);
                var results = await Collection.Business.SendEvent(@event);
                if (results.Count != 0)
                {
                    var result = (VideoDownloadEvent)results[0];
                    video.VideoUrl = result.AudioUrl;
                }
                
                break;
            }
            case ImageEntity image when !image.ImageUrl.Contains("&rkey=") && image.MsgInfo is not null:
            {
                var @event = chain.IsGroup 
                    ? ImageGroupDownloadEvent.Create(chain.GroupUin ?? 0, image.MsgInfo) 
                    : ImageDownloadEvent.Create(chain.Uid ?? string.Empty, image.MsgInfo);
                
                var results = await Collection.Business.SendEvent(@event);
                if (results.Count != 0)
                {
                    var result = (ImageDownloadEvent)results[0];
                    image.ImageUrl = result.ImageUrl;
                }
                
                break;
            }
        }
    }

    private async Task ResolveOutgoingChain(MessageChain chain)
    {
        foreach (var entity in chain) switch (entity)
        {
            case MentionEntity mention when mention.Uin != 0:
            {
                var cache = Collection.Business.CachingLogic;
                mention.Uid = await cache.ResolveUid(chain.GroupUin, mention.Uin) ?? throw new Exception($"Failed to resolve Uid for Uin {mention.Uin}");
                    
                if (chain is { IsGroup: true, GroupUin: not null } && mention.Name is null)
                {
                    var members = await Collection.Business.CachingLogic.GetCachedMembers(chain.GroupUin.Value, false);
                    var member = members.FirstOrDefault(x => x.Uin == mention.Uin);
                    if (member != null) mention.Name = $"@{member.MemberCard ?? member.MemberName}";
                }
                else if (chain is { IsGroup: false } && mention.Name is null)
                {
                    var friends = await Collection.Business.CachingLogic.GetCachedFriends(false);
                    string? friend = friends.FirstOrDefault(x => x.Uin == mention.Uin)?.Nickname;
                    if (friend != null) mention.Name = $"@{friend}";
                }
                
                break;
            }
            case MultiMsgEntity { ResId: null } multiMsg:
            {
                var multiMsgEvent = MultiMsgUploadEvent.Create(multiMsg.GroupUin, multiMsg.Chains);
                var results = await Collection.Business.SendEvent(multiMsgEvent);
                if (results.Count != 0)
                {
                    var result = (MultiMsgUploadEvent)results[0];
                    multiMsg.ResId = result.ResId;
                }
                break;
            }
            case MultiMsgEntity { ResId: not null, Chains.Count: 0 } multiMsg:
            {
                var @event = MultiMsgDownloadEvent.Create(chain.Uid ?? "", multiMsg.ResId);
                var results = await Collection.Business.SendEvent(@event);
                if (results.Count != 0)
                {
                    var result = (MultiMsgDownloadEvent)results[0];
                    multiMsg.Chains.AddRange((IEnumerable<MessageChain>?)result.Chains ?? Array.Empty<MessageChain>());
                }
                break;
            }
        }
    }

    /// <summary>
    /// <para>Resolve the <see cref="MessageChain.GroupMemberInfo"/> or <see cref="MessageChain.FriendInfo"/> for the <see cref="MessageChain"/></para>
    /// <para>for both Incoming and Outgoing MessageChain</para>
    /// </summary>
    /// <param name="chain">The target chain</param>
    private async Task ResolveChainMetadata(MessageChain chain)
    {
        if (chain is { IsGroup: true, GroupUin: not null })
        {
            var groups = await Collection.Business.CachingLogic.GetCachedMembers(chain.GroupUin.Value, false);
            chain.GroupMemberInfo = chain.FriendUin == 0 
                ? groups.FirstOrDefault(x => x.Uin == Collection.Keystore.Uin) 
                : groups.FirstOrDefault(x => x.Uin == chain.FriendUin);

            chain.Uid ??= chain.GroupMemberInfo?.Uid;
        }
        else
        {
            var friends = await Collection.Business.CachingLogic.GetCachedFriends(false);
            if (friends.FirstOrDefault(x => x.Uin == chain.FriendUin) is { } friend)
            {
                chain.FriendInfo = friend;
                chain.Uid ??= friend.Uid;
            }
        }
    }
}