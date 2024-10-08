using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.System;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.System;

[EventSubscribe(typeof(FetchFriendsRequestsEvent))]
[Service("OidbSvcTrpcTcp.0x5cf_11")]
internal class FetchFriendsRequestsService : BaseService<FetchFriendsRequestsEvent>
{
    protected override bool Build(FetchFriendsRequestsEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x5CF_11>(new OidbSvcTrpcTcp0x5CF_11
        {
            Field1 = 1,
            Field3 = 6,
            SelfUid = keystore.Uid ?? "",
            Field5 = 0,
            Field6 = 80,
            Field8 = 2,
            Field9 = 0,
            Field12 = 1,
            Field22 = 1
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchFriendsRequestsEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x5CF_11Response>>(input);
        var requests = payload.Body.Info.Requests.Select(r => new BotFriendRequest(r.TargetUid, r.SourceUid, r.State, r.Comment, r.Source, r.Timestamp)).ToList();
        
        extraEvents = null;
        output = FetchFriendsRequestsEvent.Result((int)payload.ErrorCode, requests);
        return true;
    }
}