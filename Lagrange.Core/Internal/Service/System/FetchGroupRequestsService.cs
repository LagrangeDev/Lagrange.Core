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

[EventSubscribe(typeof(FetchGroupRequestsEvent))]
[Service("OidbSvcTrpcTcp.0x10c0_1")]
internal class FetchGroupRequestsService : BaseService<FetchGroupRequestsEvent>
{
    protected override bool Build(FetchGroupRequestsEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x10C0_1>(new OidbSvcTrpcTcp0x10C0_1
        {
            Count = 20,
            Field2 = 0
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out FetchGroupRequestsEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x10C0_1Response>>(input.AsSpan());
        var events = payload.Body.Requests.Select(x => new FetchGroupRequestsEvent.RawEvent(
            x.Group.GroupUin,
            x.Invitor?.Uid,
            x.Invitor?.Name,
            x.Target.Uid,
            x.Target.Name,
            x.Operator?.Uid,
            x.Operator?.Name,
            x.Sequence,
            x.State,
            x.EventType,
            x.Comment
        )).ToList();
        
        output = FetchGroupRequestsEvent.Result((int)payload.ErrorCode, events);
        extraEvents = null;
        return true;
    }
}