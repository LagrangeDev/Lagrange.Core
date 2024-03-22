using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

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
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchCookieEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x102A_0Response>>(input);
        var cookies = packet.Body.Urls.Select(x => Encoding.UTF8.GetString(x.Value)).ToList();
        
        output = FetchCookieEvent.Result((int)packet.ErrorCode, cookies);
        extraEvents = null;
        return true;
    }
}