using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Event.Notify;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Internal.Packets.Message.Notify;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(PushMessageEvent))]
[Service("trpc.msg.olpush.OlPushService.MsgPush")]
internal class PushMessageService : BaseService<PushMessageEvent>
{
    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out PushMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var message = Serializer.Deserialize<PushMsg>(input.AsSpan());
        var packetType = (PkgType)message.Message.ContentHead.Type;
        
        output = null!;
        extraEvents = new List<ProtocolEvent>();
        switch (packetType)
        {
            case PkgType.PrivateMessage or PkgType.GroupMessage or PkgType.TempMessage:
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
            case PkgType.PrivateRecordMessage:
            {
                var chain = MessagePacker.Parse(message.Message);
                output = PushMessageEvent.Create(chain);
                break;
            }
            case PkgType.GroupRequestJoinNotice when message.Message.Body?.MsgContent is { } content:
            {
                break;
            }
            case PkgType.GroupRequestInvitationNotice when message.Message.Body?.MsgContent is { } content:
            {
                
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
                var increaseEvent = GroupSysIncreaseEvent.Result(increase.GroupUin, increase.MemberUid, increase.OperatorUid, increase.IncreaseType);
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

    private static void ProcessEvent0x2DC(byte[] payload, PushMsg msg, List<ProtocolEvent> extraEvents)
    {
        var pkgType = (Event0x2DCSubType)(msg.Message.ContentHead.SubType ?? 0);
        switch (pkgType)
        {
            case Event0x2DCSubType.GroupRecallNotice when msg.Message.Body?.MsgContent is { } content:
            {
                var subInfo = content[7..];
                Console.WriteLine(subInfo.Hex());
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
            default:
            {
                Console.WriteLine($"Unknown Event0x2DC message type: {pkgType}: {payload.Hex()}");
                break;
            }
        }
    }

    private static void ProcessEvent0x210(byte[] payload, PushMsg msg, List<ProtocolEvent> extraEvents)
    {
        var pkgType = (Event0x210SubType)(msg.Message.ContentHead.SubType ?? 0);
        switch (pkgType)
        {
            case Event0x210SubType.FriendRequestNotice when msg.Message.Body?.MsgContent is { } content:
            {
                var friend = Serializer.Deserialize<FriendRequest>(content.AsSpan());
                var friendEvent = FriendSysRequestEvent.Result(msg.Message.ResponseHead.FromUin, friend.Info.SourceUid, friend.Info.Message, friend.Info.Name);
                extraEvents.Add(friendEvent);
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
        
        Event0x210 = 528,
        Event0x2DC = 732,
        
        PrivateRecordMessage = 208,
        PrivateFileMessage = 529,
        
        GroupRequestInvitationNotice = 525, // from group member invitation
        GroupRequestJoinNotice = 84, // directly entered
        GroupInviteNotice = 87,
        GroupAdminChangedNotice = 44,
        GroupMemberIncreaseNotice = 33,
        GroupMemberDecreaseNotice = 34,
    }

    private enum Event0x2DCSubType
    {
        GroupRecallNotice = 17,
        GroupMuteNotice = 12
    }
    
    private enum Event0x210SubType
    {
        Friend = 138,
        FriendRequestNotice = 226,
    }
}