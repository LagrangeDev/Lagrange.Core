using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(FriendLikeEvent))]
[Service("OidbSvcTrpcTcp.0x7e5_104")]
internal class FriendLikeService : BaseService<FriendLikeEvent>
{
    protected override bool Build(FriendLikeEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x7E5_104>(new OidbSvcTrpcTcp0x7E5_104
        {
            TargetUid = input.TargetUid,
            Field2 = 71,
            Field3 = input.Count
        });
        output = packet.Serialize();
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out FriendLikeEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<byte[]>>(input);
        output = FriendLikeEvent.Result((int)payload.ErrorCode);

        extraEvents = null;
        return true;
    }
}