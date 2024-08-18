using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message.Action;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary.Compression;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(MultiMsgDownloadEvent))]
[Service("trpc.group.long_msg_interface.MsgService.SsoRecvLongMsg")]
internal class MultiMsgDownloadService : BaseService<MultiMsgDownloadEvent>
{
    protected override bool Build(MultiMsgDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new RecvLongMsgReq
        {
            Info = new RecvLongMsgInfo
            {
                Uid = new LongMsgUid { Uid = input.Uid },
                ResId = input.ResId,
                Acquire = true
            },
            Settings = new LongMsgSettings
            {
                Field1 = 2,
                Field2 = 0,
                Field3 = 0,
                Field4 = 0
            }
        };
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out MultiMsgDownloadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<RecvLongMsgResp>(input);
        var inflate = GZip.Inflate(packet.Result.Payload);
        var result = Serializer.Deserialize<LongMsgResult>(inflate.AsSpan());

        output = MultiMsgDownloadEvent.Result(0, result.Action.ActionData.MsgBody.Select(x => MessagePacker.Parse(x, true)).ToList());
        extraEvents = null;
        return true;
    }
}