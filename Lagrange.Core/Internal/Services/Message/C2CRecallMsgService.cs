using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.Message;

[Service("trpc.msg.msg_svc.MsgService.SsoC2CRecallMsg")]
[EventSubscribe<C2CRecallMsgEventReq>(Protocols.All)]
internal class C2CRecallMsgService : BaseService<C2CRecallMsgEventReq, C2CRecallMsgEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(C2CRecallMsgEventReq input, BotContext context)
    {
        var request = new SsoC2CRecallMsgReq
        {
            Type = 1,
            TargetUid = input.TargetUid,
            Info = new SsoC2CRecallMsgReqInfo
            {
                Sequence = input.Sequence,
                Random = input.Random,
                MessageId = 0x01000000UL << 32 | input.Random,
                Timestamp = input.Timestamp,
                Field5 = 0,
                ClientSequence = input.ClientSequence,
            },
            Settings = new SsoC2CRecallMsgReqSettings
            {
                Field1 = false,
                Field2 = false
            },
            Field6 = false
        };

        return ValueTask.FromResult(ProtoHelper.Serialize(request));
    }

    protected override ValueTask<C2CRecallMsgEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        return ValueTask.FromResult(new C2CRecallMsgEventResp());
    }
}