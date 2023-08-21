using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol.Message;
using Lagrange.Core.Core.Packets.Message.Action;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Compression;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Message;

[EventSubscribe(typeof(MultiMsgUploadEvent))]
[Service("trpc.group.long_msg_interface.MsgService.SsoSendLongMsg")]
internal class MultiMsgUploadService : BaseService<MultiMsgUploadEvent>
{
    protected override bool Build(MultiMsgUploadEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        if (input.Chains == null) throw new ArgumentNullException(nameof(input.Chains));
        
        var msgPacker = new MessagePacker(keystore.Uid ?? "");
        var msgBody = input.Chains.Select(chain => msgPacker.BuildFake(chain)).ToList();
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
                Uid = new LongMsgUid { Uid = keystore.Uid },
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
}