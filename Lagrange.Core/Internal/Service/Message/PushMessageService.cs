using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Message;
using Lagrange.Core.Internal.Event.Protocol.Notify;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Internal.Packets.Message.Notify;
using Lagrange.Core.Internal.Service.Abstraction;
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
    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out PushMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var message = Serializer.Deserialize<PushMsg>(payload.AsSpan());
        var packetType = (PkgType)message.Message.ContentHead.PkgNum!;
        
        output = null!;
        extraEvents = new List<ProtocolEvent>();
        switch (packetType)
        {
            case PkgType.PrivateMessage or PkgType.GroupMessage or PkgType.PrivateFileMessage:
            {
                var chain = MessagePacker.Parse(message.Message);
                output = PushMessageEvent.Create(chain);
                break;
            }
            case PkgType.GroupInviteNotice:
            {
                if (message.Message.Body?.MsgContent == null) return false;
                
                var invite = Serializer.Deserialize<GroupInvite>(message.Message.Body.MsgContent.AsSpan());
                var inviteEvent = GroupSysInviteEvent.Result(invite.GroupUin, invite.InvitorUid);
                extraEvents.Add(inviteEvent);
                break;
            }
            case PkgType.GroupAdminChangedNotice:
            {
                if (message.Message.Body?.MsgContent == null) return false;
                
                var admin = Serializer.Deserialize<GroupAdmin>(message.Message.Body.MsgContent.AsSpan());
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
                else return false;
                
                var adminEvent = GroupSysAdminEvent.Result(admin.GroupUin, uid, enabled);
                extraEvents.Add(adminEvent);
                break;
            }
            case PkgType.GroupMemberIncreaseNotice:
            {
                if (message.Message.Body?.MsgContent == null) return false;
                
                var increase = Serializer.Deserialize<GroupChange>(message.Message.Body.MsgContent.AsSpan());
                var increaseEvent = GroupSysIncreaseEvent.Result(increase.GroupUin, increase.MemberUid, increase.OperatorUid);
                extraEvents.Add(increaseEvent);
                break;
            }
            case PkgType.GroupMemberDecreaseNotice:
            {
                if (message.Message.Body?.MsgContent == null) return false;
                
                var decrease = Serializer.Deserialize<GroupChange>(message.Message.Body.MsgContent.AsSpan());
                var decreaseEvent = GroupSysDecreaseEvent.Result(decrease.GroupUin, decrease.MemberUid, decrease.OperatorUid);
                extraEvents.Add(decreaseEvent);
                break;
            }
            case PkgType.Event0x2DC:
            {
                ProcessEvent0x2DC(payload, message);
                break;
            }
            default:
            {
                Console.WriteLine($"Unknown message type: {packetType}: {payload.Hex()}");
                break;
            }
        }
        return true;
    }

    public static void ProcessEvent0x2DC(byte[] payload, PushMsg msg)
    {
        var pkgType = (Event0x2DCSubType)(msg.Message.ContentHead.PkgIndex ?? 0);
        switch (pkgType)
        {
            case Event0x2DCSubType.GroupRecallNotice:
            {
                if (msg.Message.Body?.MsgContent != null)
                {
                    var subInfo = msg.Message.Body?.MsgContent[7..];
                    Console.WriteLine(subInfo!.Hex());
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
    
    private enum PkgType
    {
        PrivateMessage = 166,
        PrivateFileMessage = 529,
        GroupMessage = 82,
        GroupInviteNotice = 87,
        Event0x210 = 528,
        Event0x2DC = 732,
        GroupAdminChangedNotice = 44,
        GroupMemberIncreaseNotice = 33,
        GroupMemberDecreaseNotice = 34,
    }

    private enum Event0x2DCSubType
    {
        GroupRecallNotice = 17
    }
    
    private enum Event0x210SubType
    {
        Friend = 138
    }
}