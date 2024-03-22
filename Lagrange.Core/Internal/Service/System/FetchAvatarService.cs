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

[EventSubscribe(typeof(FetchAvatarEvent))]
[Service("OidbSvcTrpcTcp.0xfe1_2")]
internal class FetchAvatarService : BaseService<FetchAvatarEvent>
{
    protected override bool Build(FetchAvatarEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var keys = new List<uint> { 20002, 27394, 20009, 20031, 101, 103, 102, 20022, 20023, 20024, 24002, 27037, 27049, 20011, 20016, 20021, 20003, 20004, 20005, 20006, 20020, 20026, 24007, 104, 105, 42432, 42362, 41756, 41757, 42257, 27372, 42315, 107, 45160, 45161, 27406, 62026 };

        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xFE1_2>(new OidbSvcTrpcTcp0xFE1_2
        {
            Uid = input.Uid,
            Field2 = 0,
            Keys = keys.Select(x => new OidbSvcTrpcTcp0xFE1_2Key { Key = x }).ToList()
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchAvatarEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0xFE1_2Response>>(input);

        output = FetchAvatarEvent.Result(0, payload.Body.Body.Uin, "");
        extraEvents = null;
        return true;
    }
}