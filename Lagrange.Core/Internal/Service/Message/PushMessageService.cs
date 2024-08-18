using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.Notify;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Internal.Packets.Message.Notify;
using Lagrange.Core.Internal.Service.Action;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
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
                var increaseEvent = GroupSysIncreaseEvent.Result(increase.GroupUin, increase.MemberUid, increase.OperatorUid, increase.DecreaseType);
                extraEvents.Add(increaseEvent);
                break;
            }
            case PkgType.GroupMemberDecreaseNotice when message.Message.Body?.MsgContent is { } content:
            {
                var decrease = Serializer.Deserialize<GroupChange>(content.AsSpan());
                var decreaseEvent = GroupSysDecreaseEvent.Result(decrease.GroupUin, decrease.MemberUid, decrease.OperatorUid, decrease.DecreaseType);
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
                Console.WriteLine($"Unknown message type: {packetType}: {input.Hex()}");
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
            case Event0x2DCSubType.GroupReactionNotice when msg.Message.Body?.MsgContent is { } content:
            {
                using var packet = new BinaryPacket(content);
                _ = packet.ReadUint();  // group uin
                _ = packet.ReadByte();  // unknown byte
                var proto = packet.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly); // proto length error

                var reaction = Serializer.Deserialize<GroupReaction>(proto);

                var group = reaction.GroupUid;
                var uid = reaction.Data.Data.Data.Data.OperatorUid;
                var type = reaction.Data.Data.Data.Data.Type;
                var sequence = reaction.Data.Data.Data.Target.Sequence;
                var code = reaction.Data.Data.Data.Data.Code;

                var groupRecallEvent = GroupSysReactionEvent.Result(group, sequence, uid, type == 1, code);
                extraEvents.Add(groupRecallEvent);

                break;
            }
            case Event0x2DCSubType.GroupRecallNotice when msg.Message.Body?.MsgContent is { } content:
            {
                using var packet = new BinaryPacket(content);
                _ = packet.ReadUint();  // group uin
                _ = packet.ReadByte();  // unknown byte
                var proto = packet.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly);
                var recall = Serializer.Deserialize<NotifyMessageBody>(proto);
                var meta = recall.Recall.RecallMessages[0];
                var groupRecallEvent = GroupSysRecallEvent.Result(recall.GroupUin, meta.AuthorUid, recall.Recall.OperatorUid, meta.Sequence, meta.Time, meta.Random);
                extraEvents.Add(groupRecallEvent);
                break;
            }
            case Event0x2DCSubType.GroupMuteNotice when msg.Message.Body?.MsgContent is { } content:
            {
                var mute = Serializer.Deserialize<GroupMute>(content.AsSpan());
                if (mute.Data.State.TargetUid == null)
                {
                    var groupMuteEvent = GroupSysMuteEvent.Result(mute.GroupUin, mute.OperatorUid, mute.Data.State.Duration == uint.MaxValue);
                    extraEvents.Add(groupMuteEvent);
                }
                else
                {
                    var memberMuteEvent = GroupSysMemberMuteEvent.Result(mute.GroupUin, mute.OperatorUid, mute.Data.State.TargetUid, mute.Data.State.Duration);
                    extraEvents.Add(memberMuteEvent);
                }
                break;
            }
            case Event0x2DCSubType.GroupEssenceNotice when msg.Message.Body?.MsgContent is { } content:
            {
                using var packet = new BinaryPacket(content);
                _ = packet.ReadUint();  // group uin
                _ = packet.ReadByte();  // unknown byte
                var proto = packet.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly);
                var essence = Serializer.Deserialize<NotifyMessageBody>(proto);
                var essenceMsg = essence.EssenceMessage;
                var groupEssenceEvent = GroupSysEssenceEvent.Result(essenceMsg.GroupUin, essenceMsg.MsgSequence,
                    essenceMsg.Random, essenceMsg.SetFlag, essenceMsg.MemberUin, essenceMsg.OperatorUin);
                extraEvents.Add(groupEssenceEvent);
                break;
            }
            case Event0x2DCSubType.GroupGreyTipNotice when msg.Message.Body?.MsgContent is { } content:
            {
                using var packet = new BinaryPacket(content);
                uint groupUin = packet.ReadUint();  // group uin
                _ = packet.ReadByte();  // unknown byte
                var proto = packet.ReadBytes(Prefix.Uint16 | Prefix.LengthOnly);
                var greyTip = Serializer.Deserialize<NotifyMessageBody>(proto);

                var templates = greyTip.GeneralGrayTip.MsgTemplParam.ToDictionary(x => x.Name, x => x.Value);

                if (greyTip.GeneralGrayTip.BusiType == 12)  // poke
                {
                    var groupPokeEvent = GroupSysPokeEvent.Result(groupUin, uint.Parse(templates["uin_str1"]), uint.Parse(templates["uin_str2"]), templates["action_str"], templates["suffix_str"]);
                    extraEvents.Add(groupPokeEvent);
                }
                break;
            }
            default:
            {
                Console.WriteLine($"Unknown Event0x2DC message type: {pkgType}: {payload.Hex()}");
                break;
            }
        }
    }

    private static void ProcessEvent0x210(Span<byte> payload, PushMsg msg, List<ProtocolEvent> extraEvents)
    {
        var pkgType = (Event0x210SubType)(msg.Message.ContentHead.SubType ?? 0);
        switch (pkgType)
        {
            case Event0x210SubType.FriendDeleteNotice:
            {
                // 0A8D010A4008AFB39FF80A1218755F54305768425A6368695A684555496253786F6F63474128AFB39FF80A3218755F54305768425A6368695A684555496253786F6F634741122108900410271827209092D9C10228F2C00330809096AF06609092D9C182808080021A260A0012220A2008001005721A0A18755F597831586B5A4E4E656E4E3141356A53423361576667
                break;
            }
            case Event0x210SubType.FriendRequestNotice when msg.Message.Body?.MsgContent is { } content:
            {
                if (Serializer.Deserialize<FriendRequest>(content.AsSpan()).Info is { } info)
                {
                    var friendEvent = FriendSysRequestEvent.Result(msg.Message.ResponseHead.FromUin, info.SourceUid, info.Message, info.Source);
                    extraEvents.Add(friendEvent);
                }
                break;
            }
            case Event0x210SubType.FriendRecallNotice when msg.Message.Body?.MsgContent is { } content:
            {
                var recall = Serializer.Deserialize<FriendRecall>(content.AsSpan());
                var recallEvent = FriendSysRecallEvent.Result(recall.Info.FromUid, recall.Info.Sequence, recall.Info.Time, recall.Info.Random);
                extraEvents.Add(recallEvent);
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

                if (greyTip.BusiType == 12)  // poke
                {
                    var groupPokeEvent = FriendSysPokeEvent.Result(uint.Parse(templates["uin_str1"]), uint.Parse(templates["uin_str2"]), templates["action_str"], templates["suffix_str"]);
                    extraEvents.Add(groupPokeEvent);
                }
                break;
            }
            default:
            {
                Console.WriteLine($"Unknown Event0x210 message type: {pkgType}: {payload.Hex()}");
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
        GroupReactionNotice = 16,
        GroupRecallNotice = 17,
        GroupEssenceNotice = 21,
        GroupGreyTipNotice = 20,
    }

    private enum Event0x210SubType
    {
        FriendRequestNotice = 35,
        FriendDeleteNotice = 39,
        FriendRecallNotice = 138,
        FriendPokeNotice = 290,
        GroupKickNotice = 212,
    }
}
