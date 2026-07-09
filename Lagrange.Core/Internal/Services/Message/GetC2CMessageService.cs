using Lagrange.Core.Common;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Internal.Services.Message;

[Service("trpc.msg.register_proxy.RegisterProxy.SsoGetC2cMsg")]
[EventSubscribe<GetC2CMessageEventReq>(Protocols.All)]
internal class GetC2CMessageService : BaseService<GetC2CMessageEventReq, GetC2CMessageEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(GetC2CMessageEventReq input, BotContext context)
    {
        var request = new SsoGetC2CMsgReq
        {
            PeerUid = input.PeerUid,
            StartSequence = input.StartSequence,
            EndSequence = input.EndSequence
        };

        return ValueTask.FromResult(ProtoHelper.Serialize(request));
    }

    protected override ValueTask<GetC2CMessageEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var response = ProtoHelper.Deserialize<SsoGetC2CMsgRsp>(input.Span);
        if (response.Retcode != 0)
        {
            throw new OperationException((int)response.Retcode, response.Message);
        }

        return ValueTask.FromResult(new GetC2CMessageEventResp(response.Messages));
    }
}