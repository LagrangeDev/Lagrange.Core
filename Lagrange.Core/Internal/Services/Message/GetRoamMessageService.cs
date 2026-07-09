using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.Message;

[EventSubscribe<GetRoamMessageEventReq>(Protocols.All)]
[Service("trpc.msg.register_proxy.RegisterProxy.SsoGetRoamMsg")]
internal class GetRoamMessageService : BaseService<GetRoamMessageEventReq, GetRoamMessageEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(GetRoamMessageEventReq input, BotContext context)
    {
        var packet = new SsoGetRoamMsgReq
        {
            PeerUid = input.PeerUid,
            Time = input.Time,
            Random = 0,
            Count = input.Count,
            Direction = 2
        };

        return ValueTask.FromResult(ProtoHelper.Serialize(packet));
    }

    protected override ValueTask<GetRoamMessageEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var packet = ProtoHelper.Deserialize<SsoGetRoamMsgRsp>(input.Span);
        return ValueTask.FromResult(new GetRoamMessageEventResp(packet.Messages));
    }
}
