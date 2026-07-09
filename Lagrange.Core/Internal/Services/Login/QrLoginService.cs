using System.Text;
using Lagrange.Core.Common;
using Lagrange.Core.Events;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Services.Login;

[EventSubscribe<VerifyCodeEventReq>(Protocols.Android)]
[EventSubscribe<CloseCodeEventReq>(Protocols.Android)]
[Service("wtlogin.qrlogin")]
internal class QrLoginService : BaseService<ProtocolEvent, ProtocolEvent>
{
    private Lazy<WtLogin> _packet = new();
    
    protected override ValueTask<ReadOnlyMemory<byte>> Build(ProtocolEvent input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));

        switch (input)
        {
            case VerifyCodeEventReq verifyCode:
            { 
                return new ValueTask<ReadOnlyMemory<byte>>(_packet.Value.BuildQrlogin19(verifyCode.K));
            }
            case CloseCodeEventReq closeCode:
            {
                return new ValueTask<ReadOnlyMemory<byte>>(closeCode.IsApproved ? _packet.Value.BuildQrlogin20(closeCode.K) : _packet.Value.BuildQrlogin22(closeCode.K));
            }
            default:
            {
                ArgumentNullException.ThrowIfNull(input);
                return new ValueTask<ReadOnlyMemory<byte>>(ReadOnlyMemory<byte>.Empty);
            }
        }
    }

    protected override ValueTask<ProtocolEvent> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));

        var wtlogin = _packet.Value.Parse(input.Span, out ushort command);
        var code2d = _packet.Value.ParseCode2dPacket(wtlogin, out ushort code2dCmd);
        var reader = new BinaryPacket(code2d);

        if (code2dCmd == 0x16)
        {
            _ = reader.Read<ushort>();
            int appId = reader.Read<int>();
            int flag = reader.Read<int>();

            return new ValueTask<ProtocolEvent>(new CloseCodeEventResp(0, string.Empty));
        }

        _ = reader.Read<ushort>();
        long uin = reader.Read<long>();
        byte state = reader.Read<byte>();
        if (state == 0)
        {
            uint timestamp = reader.Read<uint>();
            string platform = reader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
            
            if (code2dCmd == 0x13)
            {
                var tlvs = ProtocolHelper.TlvUnPack(ref reader);

                string message;
                if (tlvs.TryGetValue(0xD0, out var qrloginTlv))
                {
                    var qrlogin = ProtoHelper.Deserialize<QrLogin>(qrloginTlv);
                    state = (byte)qrlogin.ScanResult;

                    message = qrlogin.RejectInfo?.Tips ?? Encoding.UTF8.GetString(tlvs[0x03]);
                }
                else
                {
                    message = string.Empty;
                }
                
                string location = Encoding.UTF8.GetString(tlvs[0x05]);
                
                string? device = null;
                if (tlvs.TryGetValue(0xD1, out var deviceTlv))
                {
                    var deviceInfo = ProtoHelper.Deserialize<QrExtInfo>(deviceTlv);
                    device = deviceInfo.DevInfo.DevName;
                }

                return new ValueTask<ProtocolEvent>(new VerifyCodeEventResp(state, message, platform, location, device));
            }
            else
            {
                var misc110Data = reader.ReadBytes(Prefix.Int16 | Prefix.LengthOnly);
                var tlvs = ProtocolHelper.TlvUnPack(ref reader);
                string message = Encoding.UTF8.GetString(tlvs[0x36]);
            
                return new ValueTask<ProtocolEvent>(new CloseCodeEventResp(state, message));
            }
        }
        
        if (code2dCmd == 0x13)
        {
            string message = reader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
            return new ValueTask<ProtocolEvent>(new VerifyCodeEventResp(state, message, string.Empty, string.Empty, null));
        }
        else
        {
            string message = reader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
            return new ValueTask<ProtocolEvent>(new CloseCodeEventResp(state, message));
        }
    }
}