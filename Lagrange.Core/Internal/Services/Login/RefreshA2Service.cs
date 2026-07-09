using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.Login;

[EventSubscribe<RefreshA2EventReq>(Protocols.Android)]
[Service("trpc.login.ecdh.EcdhService.SsoNTLoginRefreshA2")]
internal class RefreshA2Service : BaseService<RefreshA2EventReq, RefreshA2EventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(RefreshA2EventReq input, BotContext context)
    {
        var reqBody = new NTLoginRefreshA2ReqBody
        {
            A2 = context.Keystore.WLoginSigs.A2, 
            D2 = context.Keystore.WLoginSigs.D2,
            D2Key = context.Keystore.WLoginSigs.D2Key
        };
        return ValueTask.FromResult(NTLoginCommon.EncodeAndroid(context, reqBody));
    }

    protected override ValueTask<RefreshA2EventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var state = NTLoginCommon.DecodeAndroid<NTLoginRefreshA2RspBody>(context, input, out var info, out var resp);
        if (state == NTLoginRetCode.LOGIN_SUCCESS) NTLoginCommon.SaveTicket(context, resp.Tickets);

        return new ValueTask<RefreshA2EventResp>(state switch
        {
            NTLoginRetCode.LOGIN_SUCCESS => new RefreshA2EventResp(state, null),
            _ when info is not null => new RefreshA2EventResp(state, (info.ErrorInfo.StrTipsTitle, info.ErrorInfo.StrTipsContent)),
            _ => new RefreshA2EventResp(state, null)
        });
    }
}