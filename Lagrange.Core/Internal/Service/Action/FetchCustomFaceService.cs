using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Action;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(FetchCustomFaceEvent))]
[Service("Faceroam.OpReq")]
internal class FetchCustomFaceService : BaseService<FetchCustomFaceEvent>
{
    protected override bool Build(FetchCustomFaceEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new FaceRoamRequest
        {
            Comm = new PlatInfo
            {
                ImPlat = 1,
                OsVersion = device.KernelVersion,
                QVersion = appInfo.CurrentVersion
            },
            SelfUin = keystore.Uin,
            SubCmd = 1,
            Field6 = 1
        };

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchCustomFaceEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<FaceRoamResponse>(input);
        var results = payload.UserInfo.FileName.Select(name => $"https://p.qpic.cn/{payload.UserInfo.Bid}/{keystore.Uin}/{name}/0").ToList();

        output = FetchCustomFaceEvent.Result((int)payload.RetCode, results);
        extraEvents = null;
        return true;
    }
}