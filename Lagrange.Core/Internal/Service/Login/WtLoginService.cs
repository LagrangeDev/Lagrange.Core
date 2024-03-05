using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Login;
using Lagrange.Core.Internal.Packets.Login.WtLogin.Entity;
using Lagrange.Core.Internal.Packets.Tlv;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Tencent;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Internal.Service.Login;

[EventSubscribe(typeof(WtLoginEvent))]
[Service("wtlogin.login", 10, 2)]
internal class WtLoginService : BaseService<WtLoginEvent>
{
    protected override bool Build(WtLoginEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        switch (input.EventState)
        {
            case WtLoginEvent.State.SubmitCaptcha:
                {
                    var packet = new Login0x0002(keystore, appInfo, device);
                    output = packet.ConstructPacket();
                    break;
                }
            case WtLoginEvent.State.SubmitSmsCode:
                {
                    var packet = new Login0x0007(keystore, appInfo, device);
                    output = packet.ConstructPacket();
                    break;
                }
            case WtLoginEvent.State.RequestSendSms:
                {
                    var packet = new Login0x0008(keystore, appInfo, device);
                    output = packet.ConstructPacket();
                    break;
                }
            case WtLoginEvent.State.Login:
                {
                    var packet = new Login0x0009(keystore, appInfo, device, 0); // Todo: ssoseq
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
        out WtLoginEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = BitConverter.GetBytes(input.Length, false).Concat(input).ToArray();
        var tlvs = Packets.Login.WtLogin.Entity.Login.Deserialize(new BinaryPacket(payload), keystore, out var loginCommand, out var state);

        if (state == Packets.Login.WtLogin.Entity.Login.State.Success)
        {
            var tlv119 = (Tlv119)tlvs[0x119];
            //var tlv161 = (Tlv161)tlvs[0x161];

            var decrypted = keystore.TeaImpl.Decrypt(tlv119.EncryptedTlv, keystore.Stub.TgtgtKey);
            tlvs = TlvPacker.ReadTlvCollections(new BinaryPacket(decrypted));

            var tlv16A = (Tlv16AResponse)tlvs[0x16A];
            var tlv106 = (Tlv106Response)tlvs[0x106];
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
            var tlv305 = (Tlv305)tlvs[0x305];
            var tlv143 = (Tlv143)tlvs[0x143];
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
            //keystore.Session.DeviceToken = tlv322.DeviceToken;
            keystore.Info = new()
            {
                Age = tlv11A.Age,
                Gender = tlv11A.Gender,
                Name = tlv11A.Nickname
            };
            keystore.Session.Skey = tlv120.Skey;
            keystore.Session.SuperKey = tlv16D.SuperKey;
            var tmp = new BinaryPacket(tlv512.DomainBody);
            for (int i = 0; i < tlv512.DomainCount; i++)
            {
                var host = tmp.ReadString(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
                var pskey = tmp.ReadString(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
                keystore.Session.PSkey[host] = pskey;
            }
            keystore.Session.D2Key = tlv305.D2Key;
            keystore.Session.D2 = tlv143.D2;
            keystore.Uid = tlv543.Layer1.Layer2.Uid;

            output = WtLoginEvent.Result(tlv11A.Age, tlv11A.Gender, tlv11A.Nickname);
            extraEvents = null;
            return true;
        }
        else if (state == Packets.Login.WtLogin.Entity.Login.State.CaptchaVerify)
        {
            var tlv104 = (Tlv104Response)tlvs[0x104];
            var tlv192 = (Tlv192)tlvs[0x192];
            var tlv546 = (Tlv546)tlvs[0x546];

            keystore.Session.T104 = tlv104.T104;
            keystore.Session.CaptchaUrl = tlv192.CaptchaUrl;
            keystore.Session.PowValue = new PowValue(tlv546.PowValue);

            output = WtLoginEvent.Result((int)state);
            extraEvents = null;
            return true;
        }
        else if (state == Packets.Login.WtLogin.Entity.Login.State.SmsRequired)
        {
            var tlv104 = (Tlv104Response)tlvs[0x104];
            //var tlv17B = (Tlv17B)tlvs[0x17B];

            keystore.Session.T104 = tlv104.T104;

            output = WtLoginEvent.Result((int)state);
            extraEvents = null;
            return true;
        }
        else if (state == Packets.Login.WtLogin.Entity.Login.State.DeviceLockViaSmsNewArea)
        {
            var tlv104 = (Tlv104Response)tlvs[0x104];
            var tlv174 = (Tlv174Response)tlvs[0x174];
            //var tlv204 = (Tlv204)tlvs[0x204];
            var tlv178 = (Tlv178)tlvs[0x178];
            //var tlv17D = (Tlv17D)tlvs[0x17D];
            var tlv402 = (Tlv402Response)tlvs[0x402];
            var tlv403 = (Tlv403Response)tlvs[0x403];
            //var tlv17E = (Tlv17E)tlvs[0x17E];

            keystore.Session.T104 = tlv104.T104;
            keystore.Session.T174 = tlv174.T174;
            keystore.Session.PhoneNumber = "+" + tlv178.AreaCode + tlv178.PhoneNumber;
            keystore.Session.T402 = tlv402.T402;
            keystore.Session.T403 = tlv403.T403;
            output = WtLoginEvent.Result((int)state);
            extraEvents = null;
            return true;

        }
        else if (tlvs.TryGetValue(0x146, out var tlv))
        {
            var tlv146 = (Tlv146)tlv;
            output = WtLoginEvent.Result((int)state, tlv146.Tag, tlv146.Message);
            extraEvents = null;
            return true;

        }
        else
        {
            output = WtLoginEvent.Result((int)state);
            extraEvents = null;
            return true;
        }
    }
}