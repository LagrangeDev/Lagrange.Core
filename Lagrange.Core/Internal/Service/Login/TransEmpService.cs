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
    private TransEmp transEmp;

    protected override bool Build(TransEmpEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        switch (input.EventState)
        {
            case TransEmpEvent.State.FetchQrCode:
                {
                    var packet = new TransEmp0x0031(keystore, appInfo, device);
                    transEmp = packet;
                    output = packet.ConstructPacket();
                    break;
                }
            case TransEmpEvent.State.QueryResult:
                {
                    var packet = new TransEmp0x0012(keystore, appInfo, device);
                    transEmp = packet;
                    output = packet.ConstructPacket();
                    break;
                }
            default:
                {
                    output = new BinaryPacket();
                    break;
                }
        }
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out TransEmpEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = BitConverter.GetBytes(input.Length, false).Concat(input).ToArray();
        var packet = transEmp.DeserializeBody(keystore, new BinaryPacket(payload), out ushort command);

        if (command == 0x31)
        {
            var tlvs = TransEmp0x0031.Deserialize(packet, keystore, out var signature);

            var tlv017 = (Tlv017)tlvs[0x017];
            var tlv01C = (Tlv01C)tlvs[0x01C];
            var tlv0D1 = (Tlv0D1Response)tlvs[0x0D1];

            keystore.Session.QrSign = signature;
            keystore.Session.QrUrl = tlv0D1.Url;
            keystore.Session.QrSig = tlv0D1.QrSig;

            output = TransEmpEvent.Result(tlv017.QrCode, tlv01C.ExpireSec, tlv0D1.Url);
        }
        else
        {
            var tlvs = TransEmp0x0012.Deserialize(packet, out var state);

            if (state == TransEmp.State.Confirmed)
            {
                var tlv018 = (Tlv018Response)tlvs[0x018];
                var tlv019 = (Tlv019)tlvs[0x019];
                var tlv01E = (Tlv01E)tlvs[0x01E];
                //var tlv002 = (Tlv002)tlvs[0x002];
                //var tlv007 = (Tlv007)tlvs[0x007];
                //var tlv015 = (Tlv015)tlvs[0x015];
                //var tlv0CE = (Tlv0CE)tlvs[0x0CE];

                keystore.Stub.TgtgtKey = tlv01E.TgtgtKey;
                keystore.Session.TempPassword = tlv018.TempPassword;
                keystore.Session.NoPicSig = tlv019.NoPicSig;

                output = TransEmpEvent.Result((int)state);
            }
            else
            {
                output = TransEmpEvent.Result((int)state);
            }
        }

        extraEvents = null;
        return true;
    }
}