using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupSetBotEvent))]
[Service("OidbSvcTrpcTcp.0x907d_1")]
internal class GroupSetBotService : BaseService<GroupSetBotEvent>
{
    protected override bool Build(GroupSetBotEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x907D_1>(new OidbSvcTrpcTcp0x907D_1
        {
            BotId = input.BotId,
            type = 2,
            On = input.On,
            GroupUin = input.GroupUin
        });
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

}