using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message.Action;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary.Compression;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(MultiMsgUploadEvent))]
[Service("trpc.group.long_msg_interface.MsgService.SsoSendLongMsg")]
internal class MultiMsgUploadService : BaseService<MultiMsgUploadEvent>
{
    protected override bool Build(MultiMsgUploadEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        if (input.Chains == null) throw new ArgumentNullException(nameof(input.Chains));

        var msgBody = input.Chains.Select(x => MessagePacker.BuildFake(x, keystore.Uid ?? "")).ToList();
        var longMsgResult = new LongMsgResult
        {
            Action = new List<LongMsgAction> {
                new()
                {
                    ActionCommand = "MultiMsg",
                    ActionData = new LongMsgContent { MsgBody = msgBody }
                }
            }
        };

        using var msgStream = new MemoryStream();
        Serializer.Serialize(msgStream, longMsgResult);
        var deflate = GZip.Deflate(msgStream.ToArray());

        var packet = new SendLongMsgReq
        {
            Info = new SendLongMsgInfo
            {
                Type = input.GroupUin == null ? 1u : 3u,
                Uid = new LongMsgUid { Uid = input.GroupUin?.ToString() ?? keystore.Uid },
                GroupUin = input.GroupUin,
                Payload = deflate
            },
            Settings = new LongMsgSettings
            {
                Field1 = 4,
                Field2 = 1,
                Field3 = 7,
                Field4 = 0
            }
        };

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out MultiMsgUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<SendLongMsgResp>(input);

        output = MultiMsgUploadEvent.Result(0, packet.Result.ResId);
        extraEvents = null;
        return true;
    }
}