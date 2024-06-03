using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(SetFriendRequestEvent))]
[Service("OidbSvcTrpcTcp.0xb5d_44")]
internal class SetFriendRequestService : BaseService<SetFriendRequestEvent>
{
    protected override bool Build(SetFriendRequestEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xB5D_44>(new OidbSvcTrpcTcp0xB5D_44
        {
            Accept = input.Accept ? 3u : 5u,
            TargetUid = input.TargetUid
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out SetFriendRequestEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);

        output = SetFriendRequestEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}