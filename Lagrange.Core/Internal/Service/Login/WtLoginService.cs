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
    private Packets.Login.WtLogin.Entity.Login login;

    protected override bool Build(WtLoginEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        switch (input.EventState)
        {
            case WtLoginEvent.State.SubmitCaptcha:
                {
                    var packet = new Login0x0002(keystore, appInfo, device);
                    login = packet;
                    output = packet.ConstructPacket();
                    break;
                }
            case WtLoginEvent.State.SubmitSmsCode:
                {
                    var packet = new Login0x0007(keystore, appInfo, device);
                    login = packet;
                    output = packet.ConstructPacket();
                    break;
                }
            case WtLoginEvent.State.RequestSendSms:
                {
                    var packet = new Login0x0008(keystore, appInfo, device);
                    login = packet;
                    output = packet.ConstructPacket();
                    break;
                }
            case WtLoginEvent.State.Login:
                {
                    var packet = new Login0x0009(keystore, appInfo, device, 0); // Todo: ssoseq
                    login = packet;
                    output = packet.ConstructPacket();
                    break;
                }
            case WtLoginEvent.State.LoginWithA2:
                {
                    var packet = new Login0x0009V2(keystore, appInfo, device);
                    login = packet;
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
        var tlvs = login.Deserialize(new BinaryPacket(payload), keystore, out var loginCommand, out var state);

        if (state == Packets.Login.WtLogin.Entity.Login.State.Success)
        {
            var tlv119 = (Tlv119)tlvs[0x119];
            //var tlv161 = (Tlv161)tlvs[0x161];

            var decrypted = keystore.TeaImpl.Decrypt(tlv119.EncryptedTlv, keystore.Stub.TgtgtKey);
            tlvs = TlvPacker.ReadTlvCollections(new BinaryPacket(decrypted));

            if (tlvs.TryGetValue(0x108, out var tlv108))
                keystore.Session.Ksid = ((Tlv108Response)tlv108).Ksid;
            var tlv16A = (Tlv16AResponse)tlvs[0x16A];
            keystore.Session.NoPicSig = tlv16A.NoPicSig;
            var tlv106 = (Tlv106Response)tlvs[0x106];
            keystore.Session.TempPassword = tlv106.TempPassword;
            var tlv10C = (Tlv10C)tlvs[0x10C];
            keystore.Session.Tgtgt = tlv10C.Tgtgt;
            var tlv10A = (Tlv10A)tlvs[0x10A];
            keystore.Session.Tgt = tlv10A.Tgt;
            var tlv10D = (Tlv10D)tlvs[0x10D];
            keystore.Session.TgtKey = tlv10D.TgtKey;
            //var tlv114 = (Tlv114)tlvs[0x114];
            var tlv10E = (Tlv10E)tlvs[0x10E];
            keystore.Session.StKey = tlv10E.StKey;
            var tlv103 = (Tlv103)tlvs[0x103];
            keystore.Session.StWebSig = tlv103.StWebSig;
            if (tlvs.TryGetValue(0x133, out var tlv133))
                keystore.Session.WtSessionTicket = ((Tlv133)tlv133).WtSessionTicket;
            if (tlvs.TryGetValue(0x134, out var tlv134))
                keystore.Session.WtSessionTicketKey = ((Tlv134)tlv134).WtSessionTicketKey;
            //var tlv528 = (Tlv528)tlvs[0x528];
            if (tlvs.TryGetValue(0x322, out var tlv322))
                keystore.Session.DeviceToken = ((Tlv322)tlv322).DeviceToken;
            //var tlv11D = (Tlv11D)tlvs[0x11D];
            //var tlv11F = (Tlv11F)tlvs[0x11F];
            //var Tlv138 = (Tlv138)tlvs[0x138];
            var tlv11A = (Tlv11A)tlvs[0x11A];
            keystore.Info = new()
            {
                Age = tlv11A.Age,
                Gender = tlv11A.Gender,
                Name = tlv11A.Nickname
            };
            //var tlv522 = (Tlv522)tlvs[0x522];
            //var tlv537 = (Tlv537)tlvs[0x537];
            //var tlv550 = (Tlv550)tlvs[0x550];
            //var tlv203 = (Tlv203)tlvs[0x203];
            if (tlvs.TryGetValue(0x120, out var tlv120))
                keystore.Session.Skey = ((Tlv120)tlv120).Skey;
            var tlv16D = (Tlv16D)tlvs[0x16D];
            keystore.Session.SuperKey = tlv16D.SuperKey;
            if (tlvs.TryGetValue(0x512, out var tlv512))
            {
                var tmp = new BinaryPacket(((Tlv512)tlv512).DomainBody);
                for (int i = 0; i < ((Tlv512)tlv512).DomainCount; i++)
                {
                    var host = tmp.ReadString(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
                    var pskey = tmp.ReadString(BinaryPacket.Prefix.Uint16 | BinaryPacket.Prefix.LengthOnly);
                    keystore.Session.PSkey[host] = pskey;
                }

            }
            var tlv305 = (Tlv305)tlvs[0x305];
            keystore.Session.D2Key = tlv305.D2Key;
            var tlv143 = (Tlv143)tlvs[0x143];
            keystore.Session.D2 = tlv143.D2;
            //var tlv118 = (Tlv118)tlvs[0x118];
            var tlv543 = (Tlv543)tlvs[0x543];
            keystore.Uid = tlv543.Layer1.Layer2.Uid;
            //var tlv163 = (Tlv163)tlvs[0x163];
            //var tlv138 = (Tlv138)tlvs[0x138];
            //var tlv130 = (Tlv130)tlvs[0x130];

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