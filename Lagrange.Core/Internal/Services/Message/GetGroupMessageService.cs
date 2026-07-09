using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.Message;

[EventSubscribe<GetGroupMessageEventReq>(Protocols.All)]
[Service("trpc.msg.register_proxy.RegisterProxy.SsoGetGroupMsg")]
internal class GetGroupMessageService : BaseService<GetGroupMessageEventReq, GetGroupMessageEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(GetGroupMessageEventReq input, BotContext context)
    {
        var packet = new SsoGetGroupMsg
        {
            Info = new SsoGetGroupMsgInfo
            {
                GroupUin = input.GroupUin,
                StartSequence = input.StartSequence,
                EndSequence = input.EndSequence
            },
            Filter = 1 // 1 for no filter, 2 for filter of only 10 msg within 3 days
        };
        return ValueTask.FromResult(ProtoHelper.Serialize(packet));
    }

    protected override ValueTask<GetGroupMessageEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var packet = ProtoHelper.Deserialize<SsoGetGroupMsgRsp>(input.Span);
        return ValueTask.FromResult(new GetGroupMessageEventResp(packet.Body.Messages ?? []));
    }
}