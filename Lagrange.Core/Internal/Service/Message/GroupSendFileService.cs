using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(GroupSendFileEvent))]
[Service("OidbSvcTrpcTcp.0x6d9_4")]
internal class GroupSendFileService : BaseService<GroupSendFileEvent>
{
    protected override bool Build(GroupSendFileEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D9_4>(new OidbSvcTrpcTcp0x6D9_4
        {
            Body = new OidbSvcTrpcTcp0x6D9_4Body
            {
                GroupUin = input.GroupUin,
                Type = 2,
                Info = new OidbSvcTrpcTcp0x6D9_4Info
                {
                    BusiType = 102,
                    FileId = input.FileKey,
                    Field3 = (uint)Random.Shared.Next(),
                    Field5 = true
                }
            }
        });

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out GroupSendFileEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);
        
        output = GroupSendFileEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}