using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.Notify;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Internal.Packets.Message.Notify;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(PushMessageEvent))]
[Service("trpc.msg.olpush.OlPushService.MsgPush")]
internal class PushMessageService : BaseService<PushMessageEvent>
{
    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out PushMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var message = Serializer.Deserialize<PushMsg>(input);
        var packetType = (PkgType)message.Message.ContentHead.Type;

        output = null!;
        extraEvents = new List<ProtocolEvent>();
        switch (packetType)
        {
            case PkgType.PrivateMessage or PkgType.GroupMessage or PkgType.TempMessage or PkgType.PrivateRecordMessage:
            {
                var chain = MessagePacker.Parse(message.Message);
                output = PushMessageEvent.Create(chain);
                break;
            }
            case PkgType.PrivateFileMessage:
            {
                var chain = MessagePacker.ParsePrivateFile(message.Message);
                output = PushMessageEvent.Create(chain);
                break;
            }
            case PkgType.GroupRequestJoinNotice when message.Message.Body?.MsgContent is { } content:
            {
                var join = Serializer.Deserialize<GroupJoin>(content.AsSpan());
                var joinEvent = GroupSysRequestJoinEvent.Result(join.GroupUin, join.TargetUid);
                extraEvents.Add(joinEvent);
                break;
            }
            case PkgType.GroupRequestInvitationNotice when message.Message.Body?.MsgContent is { } content:
            {
                var invitation = Serializer.Deserialize<GroupInvitation>(content.AsSpan());
                if (invitation.Cmd == 87)
                {
                    var info = invitation.Info.Inner;
                    var invitationEvent = GroupSysRequestInvitationEvent.Result(info.GroupUin, info.TargetUid, info.InvitorUid);
                    extraEvents.Add(invitationEvent);
                }
                break;
            }
            case PkgType.GroupInviteNotice when message.Message.Body?.MsgContent is { } content:
            {
                var invite = Serializer.Deserialize<GroupInvite>(content.AsSpan());
                var inviteEvent = GroupSysInviteEvent.Result(invite.GroupUin, invite.InvitorUid);
                extraEvents.Add(inviteEvent);
                break;
            }
            case PkgType.GroupAdminChangedNotice when message.Message.Body?.MsgContent is { } content:
            {
                var admin = Serializer.Deserialize<GroupAdmin>(content.AsSpan());
                bool enabled; string uid;
                if (admin.Body.ExtraEnable != null)
                {
                    enabled = true;
                    uid = admin.Body.ExtraEnable.AdminUid;
                }
                else if (admin.Body.ExtraDisable != null)
                {
                    enabled = false;
                    uid = admin.Body.ExtraDisable.AdminUid;
                }
                else
                {
                    return false;
                }

                extraEvents.Add(GroupSysAdminEvent.Result(admin.GroupUin, uid, enabled));
                break;
            }
            case PkgType.GroupMemberIncreaseNotice when message.Message.Body?.MsgContent is { } content:
            {
                var increase = Serializer.Deserialize<GroupChange>(content.AsSpan());
                var increaseEvent = GroupSysIncreaseEvent.Result(increase.GroupUin, increase.MemberUid, Encoding.UTF8.GetString(increase.Operator.AsSpan()), increase.DecreaseType);
                extraEvents.Add(increaseEvent);
                break;
            }
            case PkgType.GroupMemberDecreaseNotice when message.Message.Body?.MsgContent is { } content:
            {
                var decrease = Serializer.Deserialize<GroupChange>(content.AsSpan());
                GroupSysDecreaseEvent decreaseEvent;
                if (decrease.DecreaseType == 3) // 3 是bot自身被踢出，Operator字段会是一个protobuf
                {
                    var op = Serializer.Deserialize<OperatorInfo>(decrease.Operator.AsSpan());
                    decreaseEvent = GroupSysDecreaseEvent.Result(decrease.GroupUin, decrease.MemberUid, op.Operator.Uid, decrease.DecreaseType);
                }
                else
                {
                    decreaseEvent = GroupSysDecreaseEvent.Result(decrease.GroupUin, decrease.MemberUid, Encoding.UTF8.GetString(decrease.Operator.AsSpan()), decrease.DecreaseType);
                }
                extraEvents.Add(decreaseEvent);
                break;
            }
            case PkgType.Event0x210:
            {
                ProcessEvent0x210(input, message, extraEvents);
                break;
            }
            case PkgType.Event0x2DC:
            {
                ProcessEvent0x2DC(input, message, extraEvents);
                break;
            }
            default:
            {
                break;
            }
        }
        return true;
    }

    private static void ProcessEvent0x2DC(Span<byte> payload, PushMsg msg, List<ProtocolEvent> extraEvents)
    {
        var pkgType = (Event0x2DCSubType)(msg.Message.ContentHead.SubType ?? 0);
        switch (pkgType)
        {
            case Event0x2DCSubType.SubType16 when msg.Message.Body?.MsgContent is { } content:
            {
                using var packet = new BinaryPacket(content);
                _ = packet.ReadUint();  // group uin
                _ = packet.ReadByte();  // unknown byte
                var proto = packet.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly); // proto length error
                var msgBody = Serializer.Deserialize<NotifyMessageBody>(proto.AsSpan());
                switch ((Event0x2DCSubType16Field13)(msgBody.Field13 ?? 0))
                {
                    case Event0x2DCSubType16Field13.GroupMemberSpecialTitleNotice:
                    {
                        break;
                    }
                    case Event0x2DCSubType16Field13.GroupNameChangeNotice:
                    {
                        // 33CAE9171000450801109B85D0B70618FFFFFFFF0F2097D2AB9E032A0D08011209E686A8E7BEA46F766F680CA802D1DF18AA0118755F6C30323965684E706E4E6A6151725A55687776357551
                        var param = Serializer.Deserialize<GroupNameChange>(msgBody.EventParam.AsSpan());
                        extraEvents.Add(GroupSysNameChangeEvent.Result(msgBody.GroupUin, param.Name));
                        break;
                    }
                    case Event0x2DCSubType16Field13.GroupTodoNotice:
                    {
                        extraEvents.Add(GroupSysTodoEvent.Result(msgBody.GroupUin, msgBody.OperatorUid));
                        break;
                    }
                    case Event0x2DCSubType16Field13.GroupReactionNotice:
                    {
                        uint group = msgBody.GroupUin;
                        string uid = msgBody.Reaction.Data.Data.Data.OperatorUid;
                        uint type = msgBody.Reaction.Data.Data.Data.Type;
                        uint sequence = msgBody.Reaction.Data.Data.Target.Sequence;
                        string code = msgBody.Reaction.Data.Data.Data.Code;
                        uint count = msgBody.Reaction.Data.Data.Data.Count;
                        var groupRecallEvent = GroupSysReactionEvent.Result(group, sequence, uid, type == 1, code, count);
                        extraEvents.Add(groupRecallEvent);
                        break;
                    }
                }
                break;
            }
            case Event0x2DCSubType.GroupRecallNotice when msg.Message.Body?.MsgContent is { } content:
            {
                using var packet = new BinaryPacket(content);
                _ = packet.ReadUint();  // group uin
                _ = packet.ReadByte();  // unknown byte
                var proto = packet.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly);
                var recall = Serializer.Deserialize<NotifyMessageBody>(proto.AsSpan());
                var meta = recall.Recall.RecallMessages[0];
                var groupRecallEvent = GroupSysRecallEvent.Result(
                    recall.GroupUin,
                    meta.AuthorUid,
                    recall.Recall.OperatorUid,
                    meta.Sequence,
                    meta.Time,
                    meta.Random,
                    recall?.Recall.TipInfo?.Tip ?? ""
                );
                extraEvents.Add(groupRecallEvent);
                break;
            }
            case Event0x2DCSubType.GroupMuteNotice when msg.Message.Body?.MsgContent is { } content:
            {
                var mute = Serializer.Deserialize<GroupMute>(content.AsSpan());
                if (mute.Data.State.TargetUid == null)
                {
                    var groupMuteEvent = GroupSysMuteEvent.Result(mute.GroupUin, mute.OperatorUid, mute.Data.State.Duration != 0);
                    extraEvents.Add(groupMuteEvent);
                }
                else
                {
                    var memberMuteEvent = GroupSysMemberMuteEvent.Result(mute.GroupUin, mute.OperatorUid, mute.Data.State.TargetUid, mute.Data.State.Duration);
                    extraEvents.Add(memberMuteEvent);
                }
                break;
            }
            case Event0x2DCSubType.GroupGreyTipNotice21 when msg.Message.Body?.MsgContent is { } content:
            {
                using var packet = new BinaryPacket(content);
                _ = packet.ReadUint();  // group uin
                _ = packet.ReadByte();  // unknown byte
                var proto = packet.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly);
                var greytip = Serializer.Deserialize<NotifyMessageBody>(proto.AsSpan());

                if (greytip.Type == 27) // essence
                {
                    var essenceMsg = greytip.EssenceMessage;
                    var groupEssenceEvent = GroupSysEssenceEvent.Result(essenceMsg.GroupUin, essenceMsg.MsgSequence,
                        essenceMsg.Random, essenceMsg.SetFlag, essenceMsg.MemberUin, essenceMsg.OperatorUin);
                    extraEvents.Add(groupEssenceEvent);
                    break;
                }

                if (greytip.Type == 32) // recall poke
                {
                    var recallPoke = greytip.GroupRecallPoke;
                    var @event = GroupSysRecallPokeEvent.Result(
                        recallPoke.GroupUin,
                        recallPoke.OperatorUid,
                        recallPoke.TipsSeqId
                    );
                    extraEvents.Add(@event);
                }

                break;
            }
            case Event0x2DCSubType.GroupGreyTipNotice20 when msg.Message.Body?.MsgContent is { } content:
            {
                using var packet = new BinaryPacket(content);
                uint groupUin = packet.ReadUint();  // group uin
                _ = packet.ReadByte();  // unknown byte
                var proto = packet.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly);
                var greyTip = Serializer.Deserialize<NotifyMessageBody>(proto.AsSpan());

                var templates = greyTip.GeneralGrayTip.MsgTemplParam.ToDictionary(x => x.Name, x => x.Value);

                if (!templates.TryGetValue("action_str", out var actionStr) && !templates.TryGetValue("alt_str1", out actionStr))
                {
                    actionStr = string.Empty;
                }

                if (greyTip.GeneralGrayTip.BusiType == 12)  // poke
                {
                    var groupPokeEvent = GroupSysPokeEvent.Result(
                        groupUin,
                        uint.Parse(templates["uin_str1"]),
                        uint.Parse(templates["uin_str2"]),
                        actionStr,
                        templates["suffix_str"],
                        templates["action_img_url"],
                        greyTip.MsgSequence,
                        (ulong)msg.Message.ContentHead.Timestamp!.Value,
                        greyTip.TipsSeqId
                    );
                    extraEvents.Add(groupPokeEvent);
                }
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private static void ProcessEvent0x210(Span<byte> payload, PushMsg msg, List<ProtocolEvent> extraEvents)
    {
        var pkgType = (Event0x210SubType)(msg.Message.ContentHead.SubType ?? 0);
        switch (pkgType)
        {
            case Event0x210SubType.FriendRequestNotice when msg.Message.Body?.MsgContent is { } content:
            {
                if (Serializer.Deserialize<FriendRequest>(content.AsSpan()).Info is { } info)
                {
                    var friendEvent = FriendSysRequestEvent.Result(msg.Message.ResponseHead.FromUin, info.SourceUid, info.Message, info.Source ?? info.NewSource);
                    extraEvents.Add(friendEvent);
                }
                break;
            }
            case Event0x210SubType.GroupMemberEnterNotice when msg.Message.Body?.MsgContent is { } content:
            {
                var info = Serializer.Deserialize<GroupMemberEnter>(content.AsSpan());
                if (info is { Body.Info.Detail: { Style: { } style } detail })
                {
                    var @event = GroupSysMemberEnterEvent.Result(detail.GroupId, detail.GroupMemberUin, style.StyleId);
                    extraEvents.Add(@event);
                }
                break;
            }
            case Event0x210SubType.FriendDeleteOrPinChangedNotice when msg.Message.Body?.MsgContent is { } content: // Stupid TX
            {
                var info = Serializer.Deserialize<FriendDeleteOrPinChanged>(content.AsSpan());
                // if (info.Body.Data.Type == 5 && info.Body.Data.FriendDelete != null) // Friend Delete
                // 0A8D010A4008AFB39FF80A1218755F54305768425A6368695A684555496253786F6F63474128AFB39FF80A3218755F54305768425A6368695A684555496253786F6F634741122108900410271827209092D9C10228F2C00330809096AF06609092D9C182808080021A260A0012220A2008001005721A0A18755F597831586B5A4E4E656E4E3141356A53423361576667
                if (info.Body.Type == 7 && info.Body.PinChanged is { } data)
                {
                    var @event = SysPinChangedEvent.Result(
                        data.Body.Uid,
                        data.Body.GroupUin,
                        data.Body.Info.Timestamp.Length != 0
                    );
                    extraEvents.Add(@event);
                }
                break;
            }
            case Event0x210SubType.FriendRecallNotice when msg.Message.Body?.MsgContent is { } content:
            {
                var recall = Serializer.Deserialize<FriendRecall>(content.AsSpan());
                var recallEvent = FriendSysRecallEvent.Result(
                    recall.Info.FromUid,
                    recall.Info.ClientSequence,
                    recall.Info.Time,
                    recall.Info.Random,
                    recall.Info.TipInfo.Tip ?? ""
                );
                extraEvents.Add(recallEvent);
                break;
            }
            case Event0x210SubType.ServicePinChanged:
            {
                // 0A93010A4008E2FBEDF9051218755F667132684B3132624267306F7735637A57685A66726728E2FBEDF9053218755F667132684B3132624267306F7735637A57685A667267122308900410C70118C70120B7E1C1B50528B8CD0330A790E2B90660B7E1C1B585808080021A2A0A0012260A24080010A01F82FA011B08934E10E2FBEDF90518894E2003320B0831CADFEF010467388827180122330A0D33302E3138362E38362E31363410FE9D011A1E10900418B8CD0320B7E1C1B5858080800230C701380140E2FBEDF9054801
                break;
            }
            case Event0x210SubType.GroupKickNotice when msg.Message.Body?.MsgContent is { } content:
            {
                // 0A710A4008AFB39FF80A1218755F54305768425A6368695A684555496253786F6F63474128AFB39FF80A3218755F54305768425A6368695A684555496253786F6F634741122108900410D40118D4012090845428A0850230ECB982AF06609084D48080808080021A0A0A00120608BDCCF4E802180122340A0E33302E3137312E3135392E32333510FE9D011A1E10900418A08502209084D480808080800230D401380140AFB39FF80A4801
                break;
            }
            case Event0x210SubType.FriendPokeNotice when msg.Message.Body?.MsgContent is { } content:
            {
                var greyTip = Serializer.Deserialize<GeneralGrayTipInfo>(content.AsSpan());
                var templates = greyTip.MsgTemplParam.ToDictionary(x => x.Name, x => x.Value);

                if (!templates.TryGetValue("action_str", out var actionStr) && !templates.TryGetValue("alt_str1", out actionStr))
                {
                    actionStr = string.Empty;
                }

                if (greyTip.BusiType == 12)  // poke
                {
                    var friendPokeEvent = FriendSysPokeEvent.Result(
                        uint.Parse(templates["uin_str1"]),
                        uint.Parse(templates["uin_str2"]),
                        actionStr,
                        templates["suffix_str"],
                        templates["action_img_url"],
                        msg.Message.ResponseHead.FromUin,
                        greyTip.MsgInfo.Sequence,
                        (ulong)msg.Message.ContentHead.Timestamp!.Value,
                        greyTip.TipsSeqId
                    );
                    extraEvents.Add(friendPokeEvent);
                }
                break;
            }
            case Event0x210SubType.FriendRecallPoke when msg.Message.Body?.MsgContent is { } content:
            {
                var recall = Serializer.Deserialize<FriendRecallPokeInfo>(content.AsSpan());
                extraEvents.Add(FriendSysRecallPokeEvent.Result(recall.PeerUid, recall.OperatorUid, recall.TipsSeqId));
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private enum PkgType
    {
        PrivateMessage = 166,
        GroupMessage = 82,
        TempMessage = 141,

        Event0x210 = 528,  // friend related event
        Event0x2DC = 732,  // group related event

        PrivateRecordMessage = 208,
        PrivateFileMessage = 529,

        GroupRequestInvitationNotice = 525, // from group member invitation
        GroupRequestJoinNotice = 84, // directly entered
        GroupInviteNotice = 87,  // the bot self is being invited
        GroupAdminChangedNotice = 44,  // admin change, both on and off
        GroupMemberIncreaseNotice = 33,
        GroupMemberDecreaseNotice = 34,
    }

    private enum Event0x2DCSubType
    {
        GroupMuteNotice = 12,
        SubType16 = 16,
        GroupRecallNotice = 17,
        GroupGreyTipNotice21 = 21,
        GroupGreyTipNotice20 = 20,
    }

    private enum Event0x2DCSubType16Field13
    {
        GroupMemberSpecialTitleNotice = 6,
        GroupNameChangeNotice = 12,
        GroupTodoNotice = 23,
        GroupReactionNotice = 35,
    }

    private enum Event0x210SubType
    {
        FriendRequestNotice = 35,
        GroupMemberEnterNotice = 38,
        FriendDeleteOrPinChangedNotice = 39,
        FriendRecallNotice = 138,
        ServicePinChanged = 199, // e.g: My computer | QQ Wallet | ...
        FriendPokeNotice = 290,
        GroupKickNotice = 212,
        FriendRecallPoke = 321,
    }
}
