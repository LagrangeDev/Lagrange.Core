using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Action;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(SetStatusEvent))]
[EventSubscribe(typeof(SetCustomStatusEvent))]
[Service("trpc.qq_new_tech.status_svc.StatusService.SetStatus")]
internal class SetStatusService : BaseService<SetStatusEvent>
{
    protected override bool Build(SetStatusEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new SetStatus
        {
            Field1 = 0,
            Status = input.Status,
            ExtStatus = input.ExtStatus
        };

        if (input is SetCustomStatusEvent custom)
        {
            packet.CustomExt = new SetStatusCustomExt
            {
                FaceId = custom.FaceId,
                Text = custom.Text,
                Field3 = 1
            };
        }

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out SetStatusEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<SetStatusResponse>(input);
        extraEvents = null;
        output = payload.Message == "set status success" ? SetStatusEvent.Result(0) : SetStatusEvent.Result(-1);
        return true;
    }
}