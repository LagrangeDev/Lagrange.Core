using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(ImageExpiresEvent))]
[Service("OidbSvcTrpcTcp.0xbcb_0")]
internal class ImageExpires : BaseService<ImageExpiresEvent>
{
    protected override bool Build(ImageExpiresEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xBCB_0>(new OidbSvcTrpcTcp0xBCB_0
        {
            Body = new OidbSvcTrpcTcp0xBCB_0Body
            {
                Body = new OidbSvcTrpcTcp0xBCB_0URL
                {
                    Url = input.Url ?? ""
                }
            }
        });
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

}