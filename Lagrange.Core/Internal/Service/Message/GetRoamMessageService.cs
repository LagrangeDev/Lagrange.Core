using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message.Action;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(GetRoamMessageEvent))]
[Service("trpc.msg.register_proxy.RegisterProxy.SsoGetRoamMsg")]
internal class GetRoamMessageService : BaseService<GetRoamMessageEvent>
{
    protected override bool Build(GetRoamMessageEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new SsoGetRoamMsg
        {
            FriendUid = input.FriendUid,
            Time = input.Time,
            Random = 0,
            Count = input.Count,
            Direction = true
        };

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GetRoamMessageEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<SsoGetRoamMsgResponse>(input);
        var chains = payload.Messages.Select(x => MessagePacker.Parse(x)).ToList();
        
        extraEvents = null;
        output = GetRoamMessageEvent.Result(0, chains);
        return true;
    }
}