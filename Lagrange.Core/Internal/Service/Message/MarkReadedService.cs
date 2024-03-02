using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(MarkReadedEvent))]
[Service("trpc.msg.msg_svc.MsgService.SsoReadedReport")]
internal class MarkReadedService : BaseService<MarkReadedEvent>
{
    protected override bool Build(MarkReadedEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = input.TargetUid == null ? new SsoReadedReport
        {
            Group = new SsoReadedReportGroup
            {
                GroupUin = input.GroupUin,
                StartSequence = input.StartSequence
            }
        } : new SsoReadedReport
        {
            C2C = new SsoReadedReportC2C
            {
                TargetUid = input.TargetUid,
                Time = input.Time,
                StartSequence = input.StartSequence
            }
        };

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out MarkReadedEvent output, out List<ProtocolEvent>? extraEvents)
    {
        output = MarkReadedEvent.Result(0);
        extraEvents = null;
        return true;
    }
}