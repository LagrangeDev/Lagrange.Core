using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Login;
using Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;
using Lagrange.Core.Internal.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Internal.Service.Login;

[EventSubscribe(typeof(TransEmpEvent))]
[Service("wtlogin.trans_emp", 12, 2)]
internal class TransEmpService : BaseService<TransEmpEvent>
{
    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out TransEmpEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = BitConverter.GetBytes(input.Length, false).Concat(input).ToArray();
        var packet = TransEmp.DeserializeBody(keystore, new BinaryPacket(payload), out ushort command);
        
        if (command == 0x31)
        {
            var tlvs = TransEmp0x0031.Deserialize(packet, keystore, out var signature);

            var qrCode = ((Tlv017)tlvs[0x17]).QrCode;
            uint expiration = ((Tlv01C)tlvs[0x01C]).ExpireSec;
            string url = ((TlvQrCodeD1Resp)tlvs[0x0D1]).Url;
            string qrSig = ((TlvQrCodeD1Resp)tlvs[0x0D1]).QrSig;

            output = TransEmpEvent.Result(qrCode, expiration, url, qrSig, signature);
        }
        else
        {
            var tlvs = TransEmp0x0012.Deserialize(packet, out var state);

            if (state == TransEmp.State.Confirmed)
            {
                var tgtgtKey = ((Tlv01E)tlvs[0x1E]).TgtgtKey;
                var tempPassword = ((Tlv018Response)tlvs[0x18]).TempPassword;
                var noPicSig = ((Tlv019)tlvs[0x19]).NoPicSig;

                output = TransEmpEvent.Result((int)state, tgtgtKey, tempPassword, noPicSig);
            }
            else
            {
                output = TransEmpEvent.Result((int)state);
            }
        }

        extraEvents = null;
        return true;
    }
    
    protected override bool Build(TransEmpEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        output = input.EventState == TransEmpEvent.State.FetchQrCode
            ? new TransEmp0x0031(keystore, appInfo, device).ConstructPacket()
            : new TransEmp0x0012(keystore, appInfo, device).ConstructPacket();
        
        extraPackets = null;
        return true;
    }
}