using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Message;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

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
        if (packetType is PkgType.PrivateMessage or PkgType.GroupMessage)
        {
            var chain = MessagePacker.Parse(message.Message);
            output = PushMessageEvent.Create(chain);
        }
        else
        {
            output = null!;
            Console.WriteLine($"Unknown message type: {packetType}: {payload.Hex()}");
        }
        
        extraEvents = null;
        return true;
    }
    
    public enum PkgType
    {
        PrivateMessage = 166,
        GroupMessage = 82,
        GroupKickNotice = 34,
        GroupInviteNotice = 33,
        /// <summary>Also for FriendChangeNotice</summary>
        GroupCardChangeNotice = 528,
        GroupAdminPromoteNotice = 44,
        GroupPokeNotice = 732
    }
}