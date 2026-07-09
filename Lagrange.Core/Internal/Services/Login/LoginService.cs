using System.Diagnostics;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Cryptography;

namespace Lagrange.Core.Internal.Services.Login;

[EventSubscribe<LoginEventReq>(Protocols.PC)]
[Service("wtlogin.login", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class LoginService : BaseService<LoginEventReq, LoginEventResp>
{
    private Lazy<WtLogin> _packet = new();

    protected override ValueTask<ReadOnlyMemory<byte>> Build(LoginEventReq input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));
        
        return input.Cmd switch
        {
            LoginEventReq.Command.Tgtgt =>  new ValueTask<ReadOnlyMemory<byte>>(_packet.Value.BuildOicq09()),
            _ => throw new ArgumentOutOfRangeException(nameof(input), $"Unknown command: {input.Cmd}")
        };
    }

    protected override ValueTask<LoginEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));

        var result = Common.Parse(_packet.Value, input, context);
        return new ValueTask<LoginEventResp>(result);
    }
}

[EventSubscribe<LoginEventReq>(Protocols.Android)]
[Service("wtlogin.login", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class LoginServiceAndroid : BaseService<LoginEventReq, LoginEventResp>
{
    private Lazy<WtLogin> _packet = new();

    protected override async ValueTask<ReadOnlyMemory<byte>> Build(LoginEventReq input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));

        return input.Cmd switch
        {
            LoginEventReq.Command.Tgtgt =>  await _packet.Value.BuildOicq09Android(input.Password),
            LoginEventReq.Command.Captcha => await _packet.Value.BuildOicq02Android(input.Ticket),
            LoginEventReq.Command.FetchSMSCode => await _packet.Value.BuildOicq08Android(),
            LoginEventReq.Command.SubmitSMSCode => await _packet.Value.BuildOicq07Android(input.Code),
            _ => throw new ArgumentOutOfRangeException(nameof(input), $"Unknown command: {input.Cmd}")
        };
    }

    protected override ValueTask<LoginEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));

        var result = Common.Parse(_packet.Value, input, context);
        return new ValueTask<LoginEventResp>(result);
    }
}

file static class Common
{
    public static LoginEventResp Parse(WtLogin packet, ReadOnlyMemory<byte> input, BotContext context)
    {
        var payload = packet.Parse(input.Span, out ushort command);
        var reader = new BinaryPacket(payload);
        Debug.Assert(command == 0x810);
        
        ushort internalCmd = reader.Read<ushort>();
        byte state = reader.Read<byte>();
        var tlvs = ProtocolHelper.TlvUnPack(ref reader);

        if (tlvs.TryGetValue(0x146, out var error))
        {
            var errorReader = new BinaryPacket(error.AsSpan());
            uint _ = errorReader.Read<uint>(); // error code
            string errorTitle = errorReader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
            string errorMessage = errorReader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
            
            return new LoginEventResp(state, (errorTitle, errorMessage));
        }

        if (tlvs.TryGetValue(0x119, out var tgtgt))
        {
            TeaProvider.Decrypt(tgtgt, tgtgt, context.Keystore.WLoginSigs.TgtgtKey);
            var tlv119 = TeaProvider.CreateDecryptSpan(tgtgt);
            var tlv119Reader = new BinaryPacket(tlv119);
            var tlvCollection = ProtocolHelper.TlvUnPack(ref tlv119Reader);
            
            return new LoginEventResp(state, tlvCollection);
        }

        return new LoginEventResp(state, tlvs);
    }
}