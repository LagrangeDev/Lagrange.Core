using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(FetchGroupsEvent))]
[Service("OidbSvcTrpcTcp.0xfe5_2")]
internal class FetchGroupsService : BaseService<FetchGroupsEvent>
{
    protected override bool Build(FetchGroupsEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0FE5_2>(new OidbSvcTrpcTcp0FE5_2
        {
            Config = new OidbSvcTrpcTcp0FE5_2Config
            {
                Config1 = new OidbSvcTrpcTcp0FE5_2Config1(),
                Config2 = new OidbSvcTrpcTcp0FE5_2Config2(),
                Config3 = new OidbSvcTrpcTcp0FE5_2Config3()
            }
        }, false, true);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out FetchGroupsEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0xFE5_2Response>>(input.AsSpan());

        var groups = payload.Body.Groups.Select(raw =>
        {
            return new BotGroup(raw.GroupUin, raw.Info.GroupName, raw.Info.MemberCount, raw.Info.MemberMax);
        }).ToList();
        
        output = FetchGroupsEvent.Result(groups);
        extraEvents = null;
        return true;
    }
}