using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(FetchPinsEvent))]
[Service("OidbSvcTrpcTcp.0x12b3_0")]
internal class FetchPinsService : BaseService<FetchPinsEvent>
{
    protected override bool Build(FetchPinsEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<byte[]>(Array.Empty<byte>(), 0x12b3, 0);

        output = packet.Serialize();
        extraPackets = null;

        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out FetchPinsEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x12B3_0Response>>(input);

        if (payload.ErrorCode != 0)
        {
            output = FetchPinsEvent.Result((int)payload.ErrorCode, payload.ErrorMsg);
            extraEvents = null;
            return true;
        }

        var friendUids = payload.Body.Friends?.Select(f => f.Uid).ToList() ?? new List<string>();
        var groupUins = payload.Body.Groups?.Select(f => f.Uin).ToList() ?? new List<uint>();

        output = FetchPinsEvent.Result(friendUids, groupUins);
        extraEvents = null;
        return true;
    }
}