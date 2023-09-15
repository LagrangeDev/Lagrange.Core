using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Message;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Internal.Packets.Message.Action;
using Lagrange.Core.Internal.Service.Abstraction;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Compression;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(MultiMsgUploadEvent))]
[Service("trpc.group.long_msg_interface.MsgService.SsoSendLongMsg")]
internal class MultiMsgUploadService : BaseService<MultiMsgUploadEvent>
{
    protected override bool Build(MultiMsgUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        if (input.Chains == null) throw new ArgumentNullException(nameof(input.Chains));
        
        var msgPacker = new MessagePacker(keystore.Uid ?? "");
        var msgBody = input.Chains.Select(msgPacker.BuildFake).ToList();
        var longMsgResult = new LongMsgResult
        {
            Action = new LongMsgAction
            {
                ActionCommand = "MultiMsg",
                ActionData = new LongMsgContent { MsgBody = msgBody }
            }
        };
        
        using var msgStream = new MemoryStream();
        Serializer.Serialize(msgStream, longMsgResult);
        var deflate = GZip.Deflate(msgStream.ToArray());

        var packet = new SendLongMsgReq
        {
            Info = new SendLongMsgInfo
            {
                Type = 3,
                Uid = new LongMsgUid { Uid = input.GroupUin.ToString() ?? keystore.Uid },
                GroupUin = input.GroupUin,
                Payload = deflate
            },
            Settings = new LongMsgSettings
            {
                Field1 = 4,
                Field2 = 1,
                Field3 = 3,
                Field4 = 0
            }
        };
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out MultiMsgUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var packet = Serializer.Deserialize<SendLongMsgResp>(payload.AsSpan());
        
        output = MultiMsgUploadEvent.Result(0, packet.Result.ResId);
        extraEvents = null;
        return true;
    }
}