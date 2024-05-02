using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(FetchRKeyEvent))]
[Service("OidbSvcTrpcTcp.0x9067_202")]
internal class FetchRKeyService : BaseService<FetchRKeyEvent>
{
    protected override bool Build(FetchRKeyEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<NTV2RichMediaReq>(new NTV2RichMediaReq
        {
            ReqHead = new MultiMediaReqHead
            {
                Common = new CommonHead
                {
                    RequestId = 1,
                    Command = 202
                },
                Scene = new SceneInfo
                {
                    RequestType = 2,
                    BusinessType = 1,
                    SceneType = 0
                },
                Client = new ClientMeta { AgentType = 2 }
            },
            DownloadRKey = new DownloadRKeyReq
            {
                Types = new List<int> { 10, 20, 2 }
            }
        }, 0x9067, 202, false, true);

        extraPackets = null;
        output = packet.Serialize();
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out FetchRKeyEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<NTV2RichMediaResp>>(input);

        output = FetchRKeyEvent.Result((int)payload.ErrorCode, payload.Body.DownloadRKey.RKeys.Select(x => x.Rkey).ToList());
        extraEvents = null;
        return true;
    }
}