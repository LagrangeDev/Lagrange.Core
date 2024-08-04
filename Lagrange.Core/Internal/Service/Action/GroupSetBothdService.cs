using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupSetBothdEvent))]
[Service("OidbSvcTrpcTcp.0x112e_1")]
internal class GroupSetBothdService : BaseService<GroupSetBothdEvent>
{
    protected override bool Build(GroupSetBothdEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x112E_1>(new OidbSvcTrpcTcp0x112E_1
        {
            BotId = input.BotId,
            Seq = 11111,
            B_id = input.Data_1 ?? "",
            B_data = input.Data_2 ?? "",
            IDD = 0,
            GroupUin = input.GroupUin,
            GroupType = 1
        });
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

}