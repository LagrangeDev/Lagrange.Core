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

[EventSubscribe<ExchangeEmpEventReq>(Protocols.Android)]
[Service("wtlogin.exchange_emp", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class ExchangeEmpService : BaseService<ExchangeEmpEventReq, ExchangeEmpEventResp>
{
    private Lazy<WtLogin> _packet = new();

    protected override async ValueTask<ReadOnlyMemory<byte>> Build(ExchangeEmpEventReq input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));

        return input.Cmd switch
        {
            ExchangeEmpEventReq.Command.RefreshByA1 => await _packet.Value.BuildOicq15Android(),
            _ => throw new NotImplementedException($"Command {input.Cmd} not implemented")
        };
    }

    protected override ValueTask<ExchangeEmpEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));

        var wtlogin = _packet.Value.Parse(input.Span, out ushort command);
        var reader = new BinaryPacket(wtlogin);
        Debug.Assert(command == 0x810);
        
        ushort internalCmd = reader.Read<ushort>();
        byte state = reader.Read<byte>();
        var tlvs = ProtocolHelper.TlvUnPack(ref reader);

        if (tlvs.TryGetValue(0x119, out var tgtgt))
        {
            TeaProvider.Decrypt(tgtgt, tgtgt, internalCmd == 15 ? context.Keystore.WLoginSigs.A1Key : context.Keystore.WLoginSigs.TgtgtKey);
            var tlv119 = TeaProvider.CreateDecryptSpan(tgtgt);
            var tlv119Reader = new BinaryPacket(tlv119);
            var tlvCollection = ProtocolHelper.TlvUnPack(ref tlv119Reader);
            
            return new ValueTask<ExchangeEmpEventResp>(new ExchangeEmpEventResp(state, tlvCollection));
        }

        return new ValueTask<ExchangeEmpEventResp>(new ExchangeEmpEventResp(state, tlvs));
    }
}