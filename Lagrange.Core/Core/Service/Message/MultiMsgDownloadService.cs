using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Message.Action;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Compression;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Message;

[EventSubscribe(typeof(MultiMsgDownloadEvent))]
[Service("trpc.group.long_msg_interface.MsgService.SsoRecvLongMsg")]
internal class MultiMsgDownloadService : BaseService<MultiMsgDownloadEvent>
{
    protected override bool Build(MultiMsgDownloadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new RecvLongMsgReq
        {
            Info = new RecvLongMsgInfo
            {
                Uid = new RecvLongMsgUid { Uid = input.Uid },
                ResId = input.ResId,
                Acquire = true
            },
            Settings = new RecvLongMsgSettings
            {
                Field1 = 2,
                Field2 = 0,
                Field3 = 0,
                Field4 = 0
            }
        };
        
        var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out MultiMsgDownloadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<RecvLongMsgResp>(payload.AsSpan());
        var inflate = GZip.Inflate(packet.Result.Payload);
        var result = Serializer.Deserialize<LongMsgResult>(inflate.AsSpan());

        output = MultiMsgDownloadEvent.Result(0, result.Action.ActionData.MsgBody.Select(MessagePacker.Parse).ToList());
        extraEvents = null;
        return true;
    }
}