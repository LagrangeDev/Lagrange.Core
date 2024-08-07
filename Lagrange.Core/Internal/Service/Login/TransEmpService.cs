using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Login;
using Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;
using Lagrange.Core.Internal.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Internal.Service.Login;

[EventSubscribe(typeof(TransEmpEvent))]
[Service("wtlogin.trans_emp")]
internal class TransEmpService : BaseService<TransEmpEvent>
{
    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out TransEmpEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = BitConverter.GetBytes(input.Length, false).Concat(input.ToArray()).ToArray(); // 这写的啥
        var packet = TransEmp.DeserializeBody(keystore, new BinaryPacket(payload), out ushort command);
        
        if (command == 0x31)
        {
            var tlvs = TransEmp31.Deserialize(packet, keystore, out var signature);

            var qrCode = ((TlvQrCode17)tlvs[0x17]).QrCode;
            uint expiration = ((TlvQrCode1C)tlvs[0x01C]).ExpireSec;
            string url = ((TlvQrCodeD1Resp)tlvs[0x0D1]).Url;
            string qrSig = ((TlvQrCodeD1Resp)tlvs[0x0D1]).QrSig;

            output = TransEmpEvent.Result(qrCode, expiration, url, qrSig, signature);
        }
        else
        {
            var tlvs = TransEmp12.Deserialize(packet, out var state);

            if (state == TransEmp12.State.Confirmed)
            {
                var tgtgtKey = ((TlvQrCode1E)tlvs[0x1E]).TgtgtKey;
                var tempPassword = ((TlvQrCode18)tlvs[0x18]).TempPassword;
                var noPicSig = ((TlvQrCode19)tlvs[0x19]).NoPicSig;

                output = TransEmpEvent.Result(state, tgtgtKey, tempPassword, noPicSig);
            }
            else
            {
                output = TransEmpEvent.Result(state, null, null, null);
            }
        }

        extraEvents = null;
        return true;
    }
    
    protected override bool Build(TransEmpEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        output = input.EventState == TransEmpEvent.State.FetchQrCode
            ? new TransEmp31(keystore, appInfo, device).ConstructPacket().ToArray()
            : new TransEmp12(keystore, appInfo, device).ConstructPacket().ToArray();
        
        extraPackets = null;
        return true;
    }
}