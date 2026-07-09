using System.Security.Cryptography;
using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Services;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Cryptography;

namespace Lagrange.Core.Internal.Services.Login;

file static class PasswordLoginCommon
{
    internal static byte[] GenerateClientA1(PasswordLoginEventReq input, BotContext context)
    {
        var md5 = MD5.HashData(Encoding.UTF8.GetBytes(input.Password));
        
        var keyWriter = new BinaryPacket(stackalloc byte[16 + 4 + 4]);
        keyWriter.Write(md5);
        keyWriter.Write(0); // empty 4 bytes
        keyWriter.Write((uint)context.Keystore.Uin);
        var key = MD5.HashData(keyWriter.CreateReadOnlySpan());
        
        var plainWriter = new BinaryPacket(stackalloc byte[100]);
        plainWriter.Write<short>(4); // TGTGT Version
        plainWriter.Write(Random.Shared.Next());
        plainWriter.Write(0); // sso_version as empty
        plainWriter.Write(context.AppInfo.AppId);
        plainWriter.Write(8001);
        plainWriter.Write(context.Keystore.Uin);
        plainWriter.Write((int)DateTimeOffset.Now.ToUnixTimeSeconds());
        plainWriter.Write(0); // dummy IP Address
        plainWriter.Write<byte>(1);
        plainWriter.Write(md5);
        plainWriter.Write(context.Keystore.WLoginSigs.TgtgtKey);
        plainWriter.Write(0);  // unknown
        plainWriter.Write<byte>(1); // guidAvailable
        plainWriter.Write(context.Keystore.Guid);
        plainWriter.Write(1); // appid as dummy
        plainWriter.Write(1); // flag
        plainWriter.Write(context.Keystore.Uin.ToString(), Prefix.Int16 | Prefix.LengthOnly);

        return TeaProvider.Encrypt(plainWriter.CreateReadOnlySpan(), key);
    }
}

[EventSubscribe<PasswordLoginEventReq>(Protocols.PC)]
[Service("trpc.login.ecdh.EcdhService.SsoNTLoginPasswordLogin", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class PasswordLoginService : BaseService<PasswordLoginEventReq, PasswordLoginEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(PasswordLoginEventReq input, BotContext context)
    {
        var reqBody = new NTLoginPasswordLoginReqBody
        {
            A1 = PasswordLoginCommon.GenerateClientA1(input, context),
            Iframe = input.Captcha is { } value ? new NTLoginIframe
            {
                IframeSig = value.Item1,
                IframeRandstr = value.Item2,
                IframeSid = value.Item3
            } : null
        };
        
        return new ValueTask<ReadOnlyMemory<byte>>(NTLoginCommon.Encode(context, reqBody));
    }

    protected override ValueTask<PasswordLoginEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var state = NTLoginCommon.Decode<NTLoginPasswordLoginRspBody>(context, input, out var info, out var resp);
        if (state == NTLoginRetCode.LOGIN_SUCCESS) NTLoginCommon.SaveTicket(context, resp.Tickets);
        
        return new ValueTask<PasswordLoginEventResp>(state switch
        {
            NTLoginRetCode.LOGIN_SUCCESS => new PasswordLoginEventResp(state, null, null),
            NTLoginRetCode.LOGIN_ERROR_PROOF_WATER => new PasswordLoginEventResp(state, null, resp.SecCheck.IframeUrl),
            _ when info is not null => new PasswordLoginEventResp(state, (info.StrTipsTitle, info.StrTipsContent), info.StrJumpUrl),
            _ => new PasswordLoginEventResp(state, null, null)
        });
    }
}

[EventSubscribe<PasswordLoginEventReq>(Protocols.Android)]
[Service("trpc.login.ecdh.EcdhService.SsoNTLoginPasswordLogin", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class PasswordLoginAndroidService : BaseService<PasswordLoginEventReq, PasswordLoginEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(PasswordLoginEventReq input, BotContext context)
    {
        var reqBody = new NTLoginPasswordLoginReqBody
        {
            A1 = PasswordLoginCommon.GenerateClientA1(input, context),
            Iframe = input.Captcha is { } value ? new NTLoginIframe
            {
                IframeSig = value.Item1,
                IframeRandstr = value.Item2,
                IframeSid = value.Item3
            } : null,
            LoginProcessReq = new NTLoginLoginProcessReqBody
            {
                NeedRemindCancellatedStatus = true
            }
        };
        
        return new ValueTask<ReadOnlyMemory<byte>>(NTLoginCommon.EncodeAndroid(context, reqBody));
    }

    protected override ValueTask<PasswordLoginEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var state = NTLoginCommon.DecodeAndroid<NTLoginPasswordLoginRspBody>(context, input, out var info, out var resp);
        if (state == NTLoginRetCode.LOGIN_SUCCESS) NTLoginCommon.SaveTicket(context, resp.Tickets);
        
        return new ValueTask<PasswordLoginEventResp>(state switch
        {
            NTLoginRetCode.LOGIN_SUCCESS => new PasswordLoginEventResp(state, null, null),
            NTLoginRetCode.LOGIN_ERROR_PROOF_WATER when info is not null => new PasswordLoginEventResp(state, null, info.ErrorInfo.StrJumpUrl),
            _ when info is not null => new PasswordLoginEventResp(state, (info.ErrorInfo.StrTipsTitle, info.ErrorInfo.StrTipsContent), info.ErrorInfo.StrJumpUrl),
            _ => new PasswordLoginEventResp(state, null, null)
        });
    }
}