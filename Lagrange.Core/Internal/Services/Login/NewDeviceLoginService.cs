using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Services;

namespace Lagrange.Core.Internal.Services.Login;

[EventSubscribe<NewDeviceLoginEventReq>(Protocols.PC)]
[Service("trpc.login.ecdh.EcdhService.SsoNTLoginPasswordLoginNewDevice", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class NewDeviceLoginService : BaseService<NewDeviceLoginEventReq, NewDeviceLoginEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(NewDeviceLoginEventReq input, BotContext context)
    {
        var reqBody = new NTLoginPasswordLoginNewDeviceReqBody { NewDeviceCheckSucceedSig = input.Sig, };
        
        return new ValueTask<ReadOnlyMemory<byte>>(NTLoginCommon.Encode(context, reqBody));
    }

    protected override ValueTask<NewDeviceLoginEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var state = NTLoginCommon.Decode<NTLoginPasswordLoginNewDeviceRspBody>(context, input, out var info, out var resp);
        if (state == NTLoginRetCode.LOGIN_SUCCESS) NTLoginCommon.SaveTicket(context, resp.Tickets);
    
        return new ValueTask<NewDeviceLoginEventResp>(state switch
        {
            NTLoginRetCode.LOGIN_SUCCESS => new NewDeviceLoginEventResp(state, null),
            _ when info is not null => new NewDeviceLoginEventResp(state, (info.StrTipsTitle, info.StrTipsContent)),
            _ => new NewDeviceLoginEventResp(state, null)
        });
    }
}

[EventSubscribe<NewDeviceLoginEventReq>(Protocols.Android)]
[Service("trpc.login.ecdh.EcdhService.SsoNTLoginPasswordLoginNewDevice", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class NewDeviceLoginAndroidService : BaseService<NewDeviceLoginEventReq, NewDeviceLoginEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(NewDeviceLoginEventReq input, BotContext context)
    {
        var reqBody = new NTLoginPasswordLoginNewDeviceReqBody { NewDeviceCheckSucceedSig = input.Sig, };
        
        return new ValueTask<ReadOnlyMemory<byte>>(NTLoginCommon.EncodeAndroid(context, reqBody));
    }

    protected override ValueTask<NewDeviceLoginEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var state = NTLoginCommon.DecodeAndroid<NTLoginPasswordLoginNewDeviceRspBody>(context, input, out var info, out var resp);
        if (state == NTLoginRetCode.LOGIN_SUCCESS) NTLoginCommon.SaveTicket(context, resp.Tickets);
    
        return new ValueTask<NewDeviceLoginEventResp>(state switch
        {
            NTLoginRetCode.LOGIN_SUCCESS => new NewDeviceLoginEventResp(state, null),
            _ when info is not null => new NewDeviceLoginEventResp(state, (info.ErrorInfo.StrTipsTitle, info.ErrorInfo.StrTipsContent)),
            _ => new NewDeviceLoginEventResp(state, null)
        });
    }
}