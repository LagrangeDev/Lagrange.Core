using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Message;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Message;

[EventSubscribe(typeof(RecallPokeEvent))]
[Service("OidbSvcTrpcTcp.0xf51_1")]
internal class RecallPokeService : BaseService<RecallPokeEvent>
{
    protected override bool Build(RecallPokeEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        // I didn't find out how to get it, but a random number is feasible
        long random = !input.IsGroup
            ? Random.Shared.NextInt64(7500000000000000000, 7509999999999999999)
            : Random.Shared.NextInt64(7580000000000000000, 7580099999999999999);


        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0xF51_1>(
            new OidbSvcTrpcTcp0xF51_1
            {
                C2CMsgInfo = !input.IsGroup ? new OidbSvcTrpcTcp0xF51_1C2CMsgInfo
                {
                    AioUin = input.Uin,
                    MsgType = 5,
                    MsgSeq = input.MessageSequence,
                    MsgTime = input.MessageTime,
                    MsgUid = (ulong)random,
                } : null,
                GroupMsgInfo = input.IsGroup ? new OidbSvcTrpcTcp0xF51_1GroupMsgInfo
                {
                    GroupCode = input.Uin,
                    MsgType = 5,
                    MsgSeq = input.MessageSequence,
                    MsgTime = input.MessageTime,
                    MsgUid = (ulong)random,
                    MsgId = (ulong)random,
                } : null,
                CommGrayTipsInfo = new OidbSvcTrpcTcp0xF51_1CommGrayTipsInfo
                {
                    BusiId = 1061,
                    TipsSeqId = input.TipsSeqId,
                }
            },
            0xf51, 1
        );

        output = packet.Serialize();
        extraPackets = null;

        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out RecallPokeEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<byte[]>>(input);
        output = RecallPokeEvent.Result((int)payload.ErrorCode, payload.ErrorMsg);
        extraEvents = null;
        return true;
    }
}
