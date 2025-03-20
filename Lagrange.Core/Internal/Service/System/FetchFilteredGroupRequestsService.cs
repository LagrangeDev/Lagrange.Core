using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(FetchGroupRequestsEvent))]
[Service("OidbSvcTrpcTcp.0x10c0_2")]
internal class FetchFilteredGroupRequestsService : BaseService<FetchGroupRequestsEvent>
{
    protected override bool Build(FetchGroupRequestsEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x10C0_2>(new OidbSvcTrpcTcp0x10C0_2
        {
            Count = 20,
            Field2 = 0
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out FetchGroupRequestsEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x10C0Response>>(input);
        var events = payload.Body.Requests?.Select(x => new FetchGroupRequestsEvent.RawEvent(
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
            x.Comment,
            true
        )).ToList() ?? new List<FetchGroupRequestsEvent.RawEvent>();

        output = payload.ErrorCode == 0
            ? FetchGroupRequestsEvent.Result(events)
            : FetchGroupRequestsEvent.Result((int)payload.ErrorCode, payload.ErrorMsg);
        extraEvents = null;
        return true;
    }
}