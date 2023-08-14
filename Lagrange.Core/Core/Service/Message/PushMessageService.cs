using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Event.Protocol.Notify;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Message;
using Lagrange.Core.Core.Packets.Message.Notify;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Service.Message;

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
            case PkgType.PrivateMessage or PkgType.GroupMessage:
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
            default:
            {
                Console.WriteLine($"Unknown message type: {packetType}: {payload.Hex()}");
                break;
            }
        }
        return true;
    }
    
    private enum PkgType
    {
        PrivateMessage = 166,
        GroupMessage = 82,
        GroupKickNotice = 34,
        GroupInviteNotice = 87,
        Event0x210 = 528,
        GroupAdminChangedNotice = 44,
        GroupPokeNotice = 732
    }
}