using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupJoinEmojiChainEvent))]
[EventSubscribe(typeof(FriendJoinEmojiChainEvent))]
[Service("OidbSvcTrpcTcp.0x90ee_1")]
internal class JoinEmojiChainService : BaseService<JoinEmojiChainEvent>
{
    protected override bool Build(JoinEmojiChainEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x90EE_1>(new OidbSvcTrpcTcp0x90EE_1
        {
            FaceId = input.TargetFaceId,
            TargetMsgSeq = input.TargetMessageSeq,
            TargetMsgSeq_2 = input.TargetMessageSeq,
            Field4 = input.GroupUin == null ? 1 : 2,
            TargetGroupId = input.GroupUin,
            TargetUid = input.FriendUid,
        });
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out JoinEmojiChainEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);
        output = JoinEmojiChainEvent.Result((int)payload.ErrorCode);
        extraEvents = null;
        return true;
    }
}