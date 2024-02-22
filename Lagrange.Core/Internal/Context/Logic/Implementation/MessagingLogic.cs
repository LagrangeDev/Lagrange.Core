using Lagrange.Core.Event;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Internal.Context.Attributes;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.Notify;
using Lagrange.Core.Internal.Service;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Internal.Context.Logic.Implementation;

[EventSubscribe(typeof(PushMessageEvent))]
[EventSubscribe(typeof(SendMessageEvent))]
[EventSubscribe(typeof(GroupSysInviteEvent))]
[EventSubscribe(typeof(GroupSysAdminEvent))]
[EventSubscribe(typeof(GroupSysIncreaseEvent))]
[EventSubscribe(typeof(GroupSysDecreaseEvent))]
[EventSubscribe(typeof(FriendSysRequestEvent))]
[EventSubscribe(typeof(GroupSysMuteEvent))]
[EventSubscribe(typeof(GroupSysMemberMuteEvent))]
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
                var requestArgs = new FriendRequestEvent(info.SourceUin, info.Name, info.Message);
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
        }
    }

    public override async Task Outgoing(ProtocolEvent e)
    {
        switch (e)
        {
            case SendMessageEvent send: // resolve Uin to Uid
                await ResolveChainMetadata(send.Chain);
                await ResolveOutgoingChain(send.Chain);
                await Collection.Highway.UploadResources(send.Chain);
                break;
        }
    }

    private async Task ResolveIncomingChain(MessageChain chain)
    {
        if (chain.HasTypeOf<FileEntity>())
        {
            var file = chain.GetEntity<FileEntity>();
            if (file?.IsGroup != false || file.FileHash == null || file.FileUuid == null) return;

            var @event = FileDownloadEvent.Create(file.FileUuid, file.FileHash, chain.Uid, chain.SelfUid);
            var results = await Collection.Business.SendEvent(@event);
            if (results.Count != 0)
            {
                var result = (FileDownloadEvent)results[0];
                file.FileUrl = result.FileUrl;
            }
        }

        if (chain.HasTypeOf<MultiMsgEntity>())
        {
            var multi = chain.GetEntity<MultiMsgEntity>();
            if (multi?.ResId == null) return;

            var @event = MultiMsgDownloadEvent.Create(chain.Uid ?? "", multi.ResId);
            var results = await Collection.Business.SendEvent(@event);
            if (results.Count != 0)
            {
                var result = (MultiMsgDownloadEvent)results[0];
                multi.Chains.AddRange((IEnumerable<MessageChain>?)result.Chains ?? Array.Empty<MessageChain>());
            }
        }

        if (chain.HasTypeOf<RecordEntity>())
        {
            var record = chain.GetEntity<RecordEntity>();
            if (record?.AudioUuid == null) return;

            string uid = (chain.IsGroup
                ? await Collection.Business.CachingLogic.ResolveUid(chain.GroupUin, chain.FriendUin)
                : chain.Uid) ?? "";
            var @event = RecordDownloadEvent.Create(record.AudioUuid, uid, record.AudioName, record.FileSha1, chain.IsGroup);
            var results = await Collection.Business.SendEvent(@event);
            if (results.Count != 0)
            {
                var result = (RecordDownloadEvent)results[0];
                record.AudioUrl = result.AudioUrl;
            }
        }

        if (chain.HasTypeOf<VideoEntity>())
        {
            var video = chain.GetEntity<VideoEntity>();
            if (video?.VideoUuid == null) return;

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
        }

        foreach (var mention in chain.OfType<MentionEntity>())
        {
            if (chain is { IsGroup: true, GroupUin: not null })
            {
                var members = await Collection.Business.CachingLogic.GetCachedMembers(chain.GroupUin.Value, false);
                mention.Name ??= members.FirstOrDefault(x => x.Uin == mention.Uin)?.MemberCard;
            }
            else
            {
                var friends = await Collection.Business.CachingLogic.GetCachedFriends(false);
                mention.Name ??= friends.FirstOrDefault(x => x.Uin == mention.Uin)?.Nickname;
            }
        }
    }

    private async Task ResolveOutgoingChain(MessageChain chain)
    {
        foreach (var entity in chain)
        {
            switch (entity)
            {
                case MentionEntity mention when mention.Uin != 0:
                {
                    var cache = Collection.Business.CachingLogic;
                    mention.Uid = await cache.ResolveUid(chain.GroupUin, mention.Uin) ?? throw new Exception($"Failed to resolve Uid for Uin {mention.Uin}");

                    if (string.IsNullOrEmpty(mention.Name))
                    {
                        if (chain is { IsGroup: true, GroupUin: not null })
                        {
                            var member = (await cache.GetCachedMembers(chain.GroupUin.Value, false)).FirstOrDefault(x => x.Uin == chain.FriendUin);
                            mention.Name = member?.MemberCard;
                        }
                        else
                        {
                            var friend = (await cache.GetCachedFriends(false)).FirstOrDefault(x => x.Uin == chain.FriendUin);
                            mention.Name = friend?.Nickname;
                        }
                    }
                    break;
                }
                case MultiMsgEntity { ResId: null } multiMsg:
                {
                    foreach (var multi in multiMsg.Chains)
                    {
                        await ResolveChainMetadata(multi);
                        await Collection.Highway.UploadResources(multi);
                    }

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
        }
        else
        {
            var friends = await Collection.Business.CachingLogic.GetCachedFriends(false);
            if (friends.FirstOrDefault(x => x.Uin == chain.FriendUin) is { } friend) chain.FriendInfo = friend;
        }
    }
}