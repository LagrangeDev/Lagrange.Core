using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupInviteEvent))]
[Service("OidbSvcTrpcTcp.0x758_1")]
internal class GroupInviteService : BaseService<GroupInviteEvent>
{
    protected override bool Build(GroupInviteEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x758_1>(new OidbSvcTrpcTcp0x758_1
        {
            GroupUin = input.GroupUin,
            UidList = input.InviteUids.Select(x => new OidbSvcTrpcTcp0x758_1Uid
            {
                InviteUid = x.Key,
                SourceGroupUin = x.Value
            }).ToList(),
            Field10 = 0
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out GroupInviteEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpResponse<byte[]>>(input.AsSpan());
        
        output = GroupInviteEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}