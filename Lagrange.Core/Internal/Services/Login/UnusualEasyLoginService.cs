using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.Login;

[EventSubscribe<UnusualEasyLoginEventReq>(Protocols.PC)]
[Service("trpc.login.ecdh.EcdhService.SsoNTLoginEasyLoginUnusualDevice", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class UnusualEasyLoginService : BaseService<UnusualEasyLoginEventReq, UnusualEasyLoginEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(UnusualEasyLoginEventReq input, BotContext context)
    {
        if (context.Keystore.WLoginSigs.A1 is not { Length: > 0 } a1) throw new InvalidOperationException("A1 is not set");

        var reqBody = new NTLoginEasyLoginUnusualDeviceReqBody { A1 = a1, };
        return new ValueTask<ReadOnlyMemory<byte>>(NTLoginCommon.Encode(context, reqBody));
    }

    protected override ValueTask<UnusualEasyLoginEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var state = NTLoginCommon.Decode<NTLoginEasyLoginUnusualDeviceRspBody>(context, input, out var info, out var resp);
        if (state == NTLoginRetCode.LOGIN_SUCCESS) NTLoginCommon.SaveTicket(context, resp.Tickets);
    
        return new ValueTask<UnusualEasyLoginEventResp>(state switch
        {
            NTLoginRetCode.LOGIN_SUCCESS => new UnusualEasyLoginEventResp(state, null),
            _ when info is not null => new UnusualEasyLoginEventResp(state, (info.StrTipsTitle, info.StrTipsContent)),
            _ => new UnusualEasyLoginEventResp(state, null)
        });
    }
}