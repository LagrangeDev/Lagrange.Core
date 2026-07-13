using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.Message;

[EventSubscribe<RecallPokeEventReq>(Protocols.All)]
[Service("OidbSvcTrpcTcp.0xf51_1")]
internal class RecallPokeService : OidbService<RecallPokeEventReq, RecallPokeEventResp, DF51ReqBody, DF51RspBody>
{
    protected override uint Command => 0xf51;

    protected override uint Service => 1;

    protected override Task<DF51ReqBody> ProcessRequest(RecallPokeEventReq request, BotContext context)
    {
        ulong messageUid = request.IsGroup
            ? (ulong)Random.Shared.NextInt64(7_580_000_000_000_000_000, 7_580_099_999_999_999_999)
            : (ulong)Random.Shared.NextInt64(7_500_000_000_000_000_000, 7_509_999_999_999_999_999);

        return Task.FromResult(new DF51ReqBody
        {
            C2CMsgInfo = request.IsGroup ? null : new F51C2CMsgInfo
            {
                AioUin = request.PeerUin,
                MsgType = 5,
                MsgSeq = request.MessageSequence,
                MsgTime = request.MessageTime,
                MsgUid = messageUid
            },
            GroupMsgInfo = request.IsGroup ? new F51GroupMsgInfo
            {
                GroupCode = request.PeerUin,
                MsgType = 5,
                MsgSeq = request.MessageSequence,
                MsgTime = request.MessageTime,
                MsgUid = messageUid,
                MsgId = messageUid
            } : null,
            CommGrayTipsInfo = new F51CommGrayTipsInfo
            {
                BusiId = 1061,
                TipsSeqId = request.TipsSeqId
            }
        });
    }

    protected override Task<RecallPokeEventResp> ProcessResponse(DF51RspBody response, BotContext context) =>
        Task.FromResult(new RecallPokeEventResp());
}
