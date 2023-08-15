using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.System;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Service.Oidb;
using Lagrange.Core.Core.Packets.Service.Oidb.Request;
using Lagrange.Core.Core.Packets.Service.Oidb.Resopnse;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.System;

[EventSubscribe(typeof(FetchCookieEvent))]
[Service("OidbSvcTrpcTcp.0x102a_0")]
internal class FetchCookieService : BaseService<FetchCookieEvent>
{
    protected override bool Build(FetchCookieEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x102A_0>(new OidbSvcTrpcTcp0x102A_0
        {
            Domain = input.Domains
        });
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        
        output = new BinaryPacket(stream);
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchCookieEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x102A_0Response>>(payload.AsSpan());
        var cookies = packet.Body.Urls.Select(x => Encoding.UTF8.GetString(x.Value)).ToList();
        
        output = FetchCookieEvent.Result((int)packet.ErrorCode, cookies);
        extraEvents = null;
        return true;
    }
}