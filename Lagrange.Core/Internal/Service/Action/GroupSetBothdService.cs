using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupSetBothdEvent))]
[Service("OidbSvcTrpcTcp.0x112e_1")]
internal class GroupSetBothdService : BaseService<GroupSetBothdEvent>
{
    protected override bool Build(GroupSetBothdEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x112E_1>(new OidbSvcTrpcTcp0x112E_1
        {
            BotId = input.BotId,
            Seq = 11111,
            B_id = "",
            B_data = "",
            IDD = 0,
            GroupUin = input.GroupUin,
            GroupType = 1
        });
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

}