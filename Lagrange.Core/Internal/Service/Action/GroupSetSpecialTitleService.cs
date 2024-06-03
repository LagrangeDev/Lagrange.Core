using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupSetSpecialTitleEvent))]
[Service("OidbSvcTrpcTcp.0x8fc_2")]
internal class GroupSetSpecialTitleService : BaseService<GroupSetSpecialTitleEvent>
{
    protected override bool Build(GroupSetSpecialTitleEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x8FC>(new OidbSvcTrpcTcp0x8FC
        {
            GroupUin = input.GroupUin,
            Body = new OidbSvcTrpcTcp0x8FCBody
            {
                TargetUid = input.TargetUid,
                SpecialTitle = input.Title,
                SpecialTitleExpireTime = -1,
                UinName = input.Title
            }
        }, 0x8fc, 2);

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupSetSpecialTitleEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);
        
        output = GroupSetSpecialTitleEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}