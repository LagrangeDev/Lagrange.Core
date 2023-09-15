using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.System;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Internal.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(FetchClientKeyEvent))]
[Service("OidbSvcTrpcTcp.0x102a_1")]
internal class FetchClientKeyService : BaseService<FetchClientKeyEvent>
{
    protected override bool Build(FetchClientKeyEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x102A_1>(new OidbSvcTrpcTcp0x102A_1());

        var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out FetchClientKeyEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x102A_1Response>>(payload.AsSpan());

        output = FetchClientKeyEvent.Result(0, packet.Body.ClientKey);
        extraEvents = null;
        return true;
    }
}