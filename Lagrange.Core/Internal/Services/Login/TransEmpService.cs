using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Events;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Services.Login;

[EventSubscribe<TransEmp31EventReq>(Protocols.PC | Protocols.AndroidWatch)]
[EventSubscribe<TransEmp12EventReq>(Protocols.PC | Protocols.AndroidWatch)]
[Service("wtlogin.trans_emp", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class TransEmpService : BaseService<ProtocolEvent, ProtocolEvent>
{
    private Lazy<WtLogin> _packet = new();
    
    protected override ValueTask<ReadOnlyMemory<byte>> Build(ProtocolEvent input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));
        
        return new ValueTask<ReadOnlyMemory<byte>>(input switch
        {
            TransEmp31EventReq r => _packet.Value.BuildTransEmp31(r.UnusualSig),
            TransEmp12EventReq => _packet.Value.BuildTransEmp12(),
            _ => throw new NotSupportedException()
        });
    }

    protected override ValueTask<ProtocolEvent> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));

        var wtlogin = _packet.Value.Parse(input.Span, out _);
        var transEmp = _packet.Value.ParseCode2dPacket(wtlogin, out ushort transEmpCommand);
        var reader = new BinaryPacket(transEmp);
        
        short dummy = reader.Read<short>();
        int appId = reader.Read<int>();
        byte retCode = reader.Read<byte>();
        
        switch (transEmpCommand)
        {
            case 0x31:
            {
                var sig = reader.ReadBytes(Prefix.Int16 | Prefix.LengthOnly);
                var tlvs = ProtocolHelper.TlvUnPack(ref reader);
                var tlvD1 = ProtoHelper.Deserialize<QrExtInfo>(tlvs[0xD1]);
                
                return new ValueTask<ProtocolEvent>(new TransEmp31EventResp(tlvD1.QrUrl, tlvs[0x17], sig.ToArray()));
            }
            case 0x12:
            {
                if (retCode == 0)
                {
                    long uin = reader.Read<long>();
                    int retry = reader.Read<int>();
                    var tlvs = ProtocolHelper.TlvUnPack(ref reader);

                    return new ValueTask<ProtocolEvent>(new TransEmp12EventResp(retCode, uin, (tlvs[0x1e], tlvs[0x19], tlvs[0x18])));
                }

                return new ValueTask<ProtocolEvent>(new TransEmp12EventResp(retCode, 0, null));
            }
            default:
            {
                throw new NotSupportedException($"Unknown TransEmp command: {transEmpCommand}");
            }
        }
    }
}