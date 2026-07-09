using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.Message;

[Service("trpc.msg.msg_svc.MsgService.SsoGroupRecallMsg")]
[EventSubscribe<GroupRecallMsgEventReq>(Protocols.All)]
internal class GroupRecallMsgService : BaseService<GroupRecallMsgEventReq, GroupRecallMsgEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(GroupRecallMsgEventReq input, BotContext context)
    {
        var request = new SsoGroupRecallMsgReq
        {
            Type = 1,
            GroupUin = input.GroupUin,
            Field3 = new SsoGroupRecallMsgReqField3
            {
                Sequence = input.Sequence,
                Field3 = 0
            },
            Field4 = new SsoGroupRecallMsgReqField4 { Field1 = 0 }
        };

        return ValueTask.FromResult(ProtoHelper.Serialize(request));
    }

    protected override ValueTask<GroupRecallMsgEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        return ValueTask.FromResult(new GroupRecallMsgEventResp());
    }
}