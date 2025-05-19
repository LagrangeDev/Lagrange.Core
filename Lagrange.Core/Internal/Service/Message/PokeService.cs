using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(PokeEvent))]
[Service("OidbSvcTrpcTcp.0xed3_1")]
internal class PokeService : BaseService<PokeEvent>
{
    protected override bool Build(PokeEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcp0xED3_1
        {
            Uin = input.TargetUin ?? keystore.Uin,
            GroupUin = input.IsGroup ? input.PeerUin : 0,
            FriendUin = input.IsGroup ? 0 : input.PeerUin,
            Ext = 0
        };
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out PokeEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);

        output = PokeEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}