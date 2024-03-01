using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Message.Action;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Compression;
using Lagrange.Core.Utility.Extension;
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
        
        var msgBody = input.Chains.Select(x => MessagePacker.BuildFake(x, keystore.Uid ?? "")).ToList();
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
                Uid = new LongMsgUid { Uid = input.GroupUin?.ToString() ?? keystore.Uid },
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
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out MultiMsgUploadEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<SendLongMsgResp>(input.AsSpan());
        
        output = MultiMsgUploadEvent.Result(0, packet.Result.ResId);
        extraEvents = null;
        return true;
    }
}