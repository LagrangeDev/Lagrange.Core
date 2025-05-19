using System.Text.Json;
using System.Web;
using Lagrange.Core.Event;
using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Internal.Context.Attributes;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.Notify;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Misc;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Internal.Service;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Core.Message.Filter;
using ProtoBuf;
using FriendPokeEvent = Lagrange.Core.Event.EventArg.FriendPokeEvent;
using GroupPokeEvent = Lagrange.Core.Event.EventArg.GroupPokeEvent;

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
[EventSubscribe(typeof(GroupSysEssenceEvent))]
[EventSubscribe(typeof(GroupSysPokeEvent))]
[EventSubscribe(typeof(GroupSysReactionEvent))]
[EventSubscribe(typeof(GroupSysNameChangeEvent))]
[EventSubscribe(typeof(FriendSysRecallEvent))]
[EventSubscribe(typeof(FriendSysRequestEvent))]
[EventSubscribe(typeof(GroupSysMemberEnterEvent))]
[EventSubscribe(typeof(FriendSysPokeEvent))]
[EventSubscribe(typeof(LoginNotifyEvent))]
[EventSubscribe(typeof(MultiMsgDownloadEvent))]
[EventSubscribe(typeof(GroupSysTodoEvent))]
[EventSubscribe(typeof(SysPinChangedEvent))]
[EventSubscribe(typeof(FetchPinsEvent))]
[EventSubscribe(typeof(SetPinFriendEvent))]
[EventSubscribe(typeof(GroupSysRecallPokeEvent))]
[EventSubscribe(typeof(FriendSysRecallPokeEvent))]
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
                MessageFilter.Filter(push.Chain);

                var chain = push.Chain;

                // Intercept group invitation
                if (chain.Count == 1 && chain[0] is LightAppEntity { AppName: "com.tencent.qun.invite" } app)
                {
                    using var document = JsonDocument.Parse(app.Payload);
                    var root = document.RootElement;

                    string url = root.GetProperty("meta").GetProperty("news").GetProperty("jumpUrl").GetString()
                        ?? throw new Exception("sb tx! Is this 'com.tencent.qun.invite'?");
                    var query = HttpUtility.ParseQueryString(new Uri(url).Query);
                    uint groupUin = uint.Parse(query["groupcode"]
                        ?? throw new Exception("sb tx! Is this '/group/invite_join'?"));
                    ulong sequence = ulong.Parse(query["msgseq"]
                        ?? throw new Exception("sb tx! Is this '/group/invite_join'?"));

                    Collection.Invoker.PostEvent(new GroupInvitationEvent(groupUin, chain.FriendUin, sequence));
                    break;
                }

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
                    MessageFilter.Filter(chain);
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
                    MessageFilter.Filter(chain);
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
            case GroupSysEssenceEvent essence:
            {
                var essenceArgs = new GroupEssenceEvent(essence.GroupUin, essence.Sequence, essence.Random, essence.SetFlag, essence.FromUin, essence.OperatorUin);
                Collection.Invoker.PostEvent(essenceArgs);
                break;
            }
            case GroupSysPokeEvent poke:
            {
                var pokeArgs = new GroupPokeEvent(
                    poke.GroupUin,
                    poke.OperatorUin,
                    poke.TargetUin,
                    poke.Action,
                    poke.Suffix,
                    poke.ActionImgUrl,
                    poke.MessageSequence,
                    poke.MessageTime,
                    poke.TipsSeqId
                );
                Collection.Invoker.PostEvent(pokeArgs);
                break;
            }
            case GroupSysReactionEvent reaction:
            {
                uint operatorUin = await Collection.Business.CachingLogic.ResolveUin(reaction.TargetGroupUin, reaction.OperatorUid) ?? 0;
                var pokeArgs = new GroupReactionEvent(reaction.TargetGroupUin, reaction.TargetSequence, operatorUin, reaction.IsAdd, reaction.Code, reaction.Count);
                Collection.Invoker.PostEvent(pokeArgs);
                break;
            }
            case GroupSysNameChangeEvent nameChange:
            {
                var pokeArgs = new GroupNameChangeEvent(nameChange.GroupUin, nameChange.Name);
                Collection.Invoker.PostEvent(pokeArgs);
                break;
            }
            case FriendSysRequestEvent info:
            {
                var requestArgs = new FriendRequestEvent(info.SourceUin, info.SourceUid, info.Message, info.Source);
                Collection.Invoker.PostEvent(requestArgs);
                break;
            }
            case GroupSysMemberEnterEvent info:
            {
                var @event = new GroupMemberEnterEvent(info.GroupUin, info.GroupMemberUin, info.StyleId);
                Collection.Invoker.PostEvent(@event);
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
                var recallArgs = new GroupRecallEvent(recall.GroupUin, authorUin, operatorUin, recall.Sequence, recall.Time, recall.Random, recall.Tip);
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
                var recallArgs = new FriendRecallEvent(fromUin, recall.ClientSequence, recall.Time, recall.Random, recall.Tip);
                Collection.Invoker.PostEvent(recallArgs);
                break;
            }
            case FriendSysPokeEvent poke:
            {
                var pokeArgs = new FriendPokeEvent(
                    poke.OperatorUin,
                    poke.TargetUin,
                    poke.Action,
                    poke.Suffix,
                    poke.ActionImgUrl,
                    poke.PeerUin,
                    poke.MessageSequence,
                    poke.MessageTime,
                    poke.TipsSeqId
                );
                Collection.Invoker.PostEvent(pokeArgs);
                break;
            }
            case LoginNotifyEvent login:
            {
                var deviceArgs = new DeviceLoginEvent(login.IsLogin, login.AppId, login.Tag, login.Message);
                Collection.Invoker.PostEvent(deviceArgs);
                break;
            }
            case MultiMsgDownloadEvent multi:
            {
                if (multi.Chains != null)
                {
                    foreach (var chain in multi.Chains)
                    {
                        if (chain.Count == 0) continue;
                        await ResolveIncomingChain(chain);
                        MessageFilter.Filter(chain);
                    }
                }
                break;
            }
            case GroupSysTodoEvent todo:
            {
                uint uin = await Collection.Business.CachingLogic.ResolveUin(todo.GroupUin, todo.OperatorUid) ?? 0;

                Collection.Invoker.PostEvent(new GroupTodoEvent(todo.GroupUin, uin));
                break;
            }
            case SysPinChangedEvent pin:
            {
                uint uin = pin.GroupUin ?? await Collection.Business.CachingLogic.ResolveUin(null, pin.Uid) ?? 0;

                Collection.Invoker.PostEvent(new PinChangedEvent(
                    pin.GroupUin == null ? PinChangedEvent.ChatType.Friend : PinChangedEvent.ChatType.Group,
                    uin,
                    pin.IsPin
                ));
                break;
            }
            case FetchPinsEvent pins:
            {
                foreach (var friendUid in pins.FriendUids)
                {
                    pins.FriendUins.Add(await Collection.Business.CachingLogic.ResolveUin(null, friendUid) ?? 0);
                }

                break;
            }
            case GroupSysRecallPokeEvent recall:
            {
                uint operatorUin = await Collection.Business.CachingLogic.ResolveUin(null, recall.OperatorUid) ?? 0;
                var @event = new GroupRecallPokeEvent(recall.GroupUin, operatorUin, recall.TipsSeqId);
                Collection.Invoker.PostEvent(@event);
                break;
            }
            case FriendSysRecallPokeEvent recall:
            {
                uint peerUin = await Collection.Business.CachingLogic.ResolveUin(null, recall.PeerUid) ?? 0;
                uint operatorUin = await Collection.Business.CachingLogic.ResolveUin(null, recall.OperatorUid) ?? 0;
                var @event = new FriendRecallPokeEvent(peerUin, operatorUin, recall.TipsSeqId);
                Collection.Invoker.PostEvent(@event);
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

                    foreach (var forward in chain.OfType<ForwardEntity>())
                    {
                        if (chain.IsGroup) await Collection.Highway.UploadGroupResources(forward.Chain, chain.GroupUin ?? 0);
                        else await Collection.Highway.UploadPrivateResources(forward.Chain, chain.FriendInfo?.Uid ?? "");
                    }
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
            case SetPinFriendEvent pinFriend: // resolve Uin to Uid
            {
                pinFriend.Uid = await Collection.Business.CachingLogic.ResolveUid(null, pinFriend.Uin)
                    ?? throw new Exception();

                break;
            }
        }
    }

    private async Task ResolveIncomingChain(MessageChain chain)
    {
        foreach (var entity in chain)
        {
            switch (entity)
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
                    MediaDownloadEvent @event = chain.IsGroup
                        ? RecordGroupDownloadEvent.Create(record.MsgInfo.MsgInfoBody[0].Index)
                        : RecordDownloadEvent.Create(record.MsgInfo.MsgInfoBody[0].Index);

                    var results = await Collection.Business.SendEvent(@event);
                    if (results.Count != 0)
                    {
                        var result = (MediaDownloadEvent)results[0];
                        record.AudioUrl = result.Url;
                    }

                    break;
                }
                case RecordEntity { AudioUuid: not null } record:
                {
                    int remainder = record.AudioUuid.Length % 4;
                    int length = remainder == 0 ? record.AudioUuid.Length : record.AudioUuid.Length + (4 - remainder);
                    string base64 = record.AudioUuid.Replace('-', '+').Replace('_', '/').PadRight(length, '=');
                    var info = Serializer.Deserialize<FileId>(Convert.FromBase64String(base64).AsSpan());

                    var index = new IndexNode
                    {
                        FileUuid = record.AudioUuid,
                        StoreId = 1,
                        UploadTime = 0,
                        Ttl = info.Ttl,
                        SubType = 0
                    };

                    var results = await Collection.Business.SendEvent(info.AppId switch
                    {
                        1402 => RecordDownloadEvent.Create(index),
                        1403 => RecordGroupDownloadEvent.Create(index),

                        _ => throw new NotSupportedException($"Unsupported Record AppId: {info.AppId}"),
                    });

                    if (results.Count != 0)
                    {
                        var result = (MediaDownloadEvent)results[0];
                        record.AudioUrl = result.Url;
                    }

                    break;
                }
                case VideoEntity video when !video.VideoUrl.Contains("&rkey=") && video.MsgInfo is not null:
                {
                    MediaDownloadEvent @event = video.IsGroup
                        ? VideoGroupDownloadEvent.Create(video.MsgInfo.MsgInfoBody[0].Index)
                        : VideoDownloadEvent.Create(video.MsgInfo.MsgInfoBody[0].Index);

                    var results = await Collection.Business.SendEvent(@event);
                    if (results.Count != 0)
                    {
                        var result = (MediaDownloadEvent)results[0];
                        video.VideoUrl = result.Url;
                    }

                    break;
                }
                case ImageEntity image when !image.ImageUrl.Contains("&rkey=") && image.MsgInfo is not null:
                {
                    MediaDownloadEvent @event = image.IsGroup
                        ? ImageGroupDownloadEvent.Create(image.MsgInfo.MsgInfoBody[0].Index)
                        : ImageDownloadEvent.Create(image.MsgInfo.MsgInfoBody[0].Index);

                    var results = await Collection.Business.SendEvent(@event);
                    if (results.Count != 0)
                    {
                        var result = (MediaDownloadEvent)results[0];
                        image.ImageUrl = result.Url;
                    }

                    break;
                }
                case ForwardEntity forward:
                {
                    if (chain is { GroupUin: not null })
                    {
                        var events = await Collection.Business.SendEvent(GetGroupMessageEvent.Create(
                            chain.GroupUin.Value,
                            forward.Sequence,
                            forward.Sequence
                        ));

                        if (events.Count < 1) break;
                        if (events[0] is not GetGroupMessageEvent @event) break;
                        if (@event.ResultCode != 0) break;
                        if (@event.Chains.Count < 1) break;

                        forward.Chain = @event.Chains[0];
                    }
                    else
                    {
                        var events = await Collection.Business.SendEvent(GetC2cMessageEvent.Create(
                            chain.Uid ?? "",
                            forward.Sequence,
                            forward.Sequence
                        ));

                        if (events.Count < 1) break;
                        if (events[0] is not GetC2cMessageEvent @event) break;
                        if (@event.ResultCode != 0) break;
                        if (@event.Chains.Count < 1) break;

                        forward.Chain = @event.Chains[0];
                    }

                    break;
                }
            }
        }
    }

    private async Task ResolveOutgoingChain(MessageChain chain)
    {
        foreach (var entity in chain)
        {
            switch (entity)
            {
                case FaceEntity face:
                {
                    var cache = Collection.Business.CachingLogic;
                    face.SysFaceEntry ??= await cache.GetCachedFaceEntity(face.FaceId);
                    break;
                }
                case BounceFaceEntity bounceFace:
                {
                    var cache = Collection.Business.CachingLogic;

                    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                    if (bounceFace.Name != null)
                        break;

                    string name = (await cache.GetCachedFaceEntity(bounceFace.FaceId))?.QDes ?? string.Empty;

                    // Because the name is used as a preview text, it should not start with '/'
                    // But the QDes of the face may start with '/', so remove it
                    if (name.StartsWith('/'))
                        name = name[1..];

                    bounceFace.Name = name;
                    break;
                }
                case ForwardEntity forward when forward.TargetUin != 0:
                {
                    var cache = Collection.Business.CachingLogic;
                    forward.Uid = await cache.ResolveUid(chain.GroupUin, forward.TargetUin) ?? throw new Exception($"Failed to resolve Uid for Uin {forward.TargetUin}");

                    break;
                }
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
                    if (chain.GroupUin != null) foreach (var c in multiMsg.Chains) c.GroupUin = chain.GroupUin;

                    var multiMsgEvent = MultiMsgUploadEvent.Create(chain.GroupUin, multiMsg.Chains);
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
