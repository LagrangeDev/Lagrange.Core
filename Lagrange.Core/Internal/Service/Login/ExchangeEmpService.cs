using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Login;
using Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;
using Lagrange.Core.Internal.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Internal.Service.Login;

[EventSubscribe(typeof(ExchangeEmpEvent))]
[Service("wtlogin.exchange_emp", 10, 2)]
internal class ExchangeEmpService : BaseService<ExchangeEmpEvent>
{
    protected override bool Build(ExchangeEmpEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        switch (input.EventState)
        {
            case ExchangeEmpEvent.State.RefreshToken:
                {
                    var packet = new ExchangeEmp0x000F(keystore, appInfo, device, 0); // Todo: ssoseq
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
        out ExchangeEmpEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = BitConverter.GetBytes(input.Length, false).Concat(input).ToArray();
        var tlvs = ExchangeEmp.Deserialize(new BinaryPacket(payload), keystore, out var loginCommand, out var state);

        if (state == ExchangeEmp.State.Success)
        {
            var tlv119 = (Tlv119)tlvs[0x119];
            //var tlv161 = (Tlv161)tlvs[0x161];

            var decrypted = keystore.TeaImpl.Decrypt(tlv119.EncryptedTlv, keystore.Session.Tgtgt);
            tlvs = TlvPacker.ReadTlvCollections(new BinaryPacket(decrypted));

            var tlv106 = (Tlv106Response)tlvs[0x106];
            var tlv16A = (Tlv16AResponse)tlvs[0x16A];
            var tlv10C = (Tlv10C)tlvs[0x10C];
            var tlv10A = (Tlv10A)tlvs[0x10A];
            var tlv10D = (Tlv10D)tlvs[0x10D];
            //var tlv114 = (Tlv114)tlvs[0x114];
            var tlv10E = (Tlv10E)tlvs[0x10E];
            var tlv103 = (Tlv103)tlvs[0x103];
            var tlv133 = (Tlv133)tlvs[0x133];
            var tlv134 = (Tlv134)tlvs[0x134];
            //var tlv528 = (Tlv528)tlvs[0x528];
            //var tlv322 = (Tlv322)tlvs[0x322];
            //var tlv11D = (Tlv11D)tlvs[0x11D];
            //var tlv11F = (Tlv11F)tlvs[0x11F];
            //var Tlv138 = (Tlv138)tlvs[0x138];
            var tlv11A = (Tlv11A)tlvs[0x11A];
            //var tlv522 = (Tlv522)tlvs[0x522];
            //var tlv537 = (Tlv537)tlvs[0x537];
            //var tlv550 = (Tlv550)tlvs[0x550];
            //var tlv203 = (Tlv203)tlvs[0x203];
            var tlv120 = (Tlv120)tlvs[0x120];
            var tlv16D = (Tlv16D)tlvs[0x16D];
            var tlv512 = (Tlv512)tlvs[0x512];
            //var tlv118 = (Tlv118)tlvs[0x118];
            var tlv543 = (Tlv543)tlvs[0x543];
            //var tlv163 = (Tlv163)tlvs[0x163];
            //var tlv138 = (Tlv138)tlvs[0x138];
            //var tlv130 = (Tlv130)tlvs[0x130];


            keystore.Session.NoPicSig = tlv16A.NoPicSig;
            keystore.Session.TempPassword = tlv106.TempPassword;
            keystore.Session.Tgtgt = tlv10C.Tgtgt;
            keystore.Session.Tgt = tlv10A.Tgt;
            keystore.Session.TgtKey = tlv10D.TgtKey;
            keystore.Session.StKey = tlv10E.StKey;
            keystore.Session.StWebSig = tlv103.StWebSig;
            keystore.Session.WtSessionTicket = tlv133.WtSessionTicket;
            keystore.Session.WtSessionTicketKey = tlv134.WtSessionTicketKey;
            keystore.Info = new()
            {
                Age = tlv11A.Age,
                Gender = tlv11A.Gender,
                Name = tlv11A.Nickname
            };
            //keystore.Session.DeviceToken = tlv322.DeviceToken;
            keystore.Session.Skey = tlv120.Skey;
            keystore.Session.SuperKey = tlv16D.SuperKey;
            var tmp = new BinaryPacket(tlv512.DomainBody);
            for (int i = 0; i < tlv512.DomainCount; i++)
            {
                var host = tmp.ReadString(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
                var pskey = tmp.ReadString(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
                keystore.Session.PSkey[host] = pskey;
            }
            keystore.Uid = tlv543.Layer1.Layer2.Uid;

            output = ExchangeEmpEvent.Result(tlv11A.Age, tlv11A.Gender, tlv11A.Nickname);
            extraEvents = null;
            return true;
        }
        else if (tlvs.TryGetValue(0x146, out var tlv))
        {
            var tlv146 = (Tlv146)tlv;
            output = ExchangeEmpEvent.Result((int)state, tlv146.Tag, tlv146.Message);
            extraEvents = null;
            return true;

        }
        else
        {
            output = ExchangeEmpEvent.Result((int)state);
            extraEvents = null;
            return true;
        }

    }
}