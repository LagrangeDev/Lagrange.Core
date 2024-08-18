using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Action;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(FetchMarketFaceKeyEvent))]
[Service("BQMallSvc.TabOpReq")]
internal class FetchMarketFaceKeyService : BaseService<FetchMarketFaceKeyEvent>
{
    protected override bool Build(FetchMarketFaceKeyEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new MarketFaceKeyRequest
        {
            Field1 = 3,
            Info = new MarketFaceInfo
            {
                FaceIds = input.FaceIds
            },
        };

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out FetchMarketFaceKeyEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<MarketFaceKeyResponse>(input);

        output = FetchMarketFaceKeyEvent.Result(0, payload.Info.Keys);
        extraEvents = null;
        return true;
    }
}