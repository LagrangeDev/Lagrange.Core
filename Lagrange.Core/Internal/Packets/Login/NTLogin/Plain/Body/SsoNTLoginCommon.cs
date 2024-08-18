using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Universal;
using Lagrange.Core.Utility.Crypto;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Body;

// ReSharper disable InconsistentNaming

internal static class SsoNTLoginCommon
{
    public static Span<byte> BuildNTLoginPacket(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, byte[] body)
    {
        if (keystore.Session.ExchangeKey == null || keystore.Session.KeySign == null) throw new InvalidOperationException("Key is null");

        var packet = new SsoNTLoginBase<SsoNTLoginEasyLogin>
        {
            Header = new SsoNTLoginHeader
            {
                Uin = new SsoNTLoginUin { Uin = keystore.Uin.ToString() },
                System = new SsoNTLoginSystem
                {
                    Os = appInfo.Os,
                    DeviceName = device.DeviceName,
                    Type = appInfo.NTLoginType,
                    Guid = device.Guid.ToByteArray()
                },
                Version = new SsoNTLoginVersion
                {
                    KernelVersion = device.KernelVersion,
                    AppId = appInfo.AppId,
                    PackageName = appInfo.PackageName
                },
                Cookie = new SsoNTLoginCookie { Cookie = keystore.Session.UnusualCookies }
            },
            Body = new SsoNTLoginEasyLogin { TempPassword = body, }
        };

        if (keystore.Session.Captcha is not null)
        {
            var (ticket, randStr, aid) = keystore.Session.Captcha.Value;
            packet.Body.Captcha = new SsoNTLoginCaptchaSubmit
            {
                Ticket = ticket,
                RandStr = randStr,
                Aid = aid
            };
        }
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        var encrypted = AesGcmImpl.Encrypt(stream.ToArray(), keystore.Session.ExchangeKey);

        var encryptPacket = new SsoNTLoginEncryptedData
        {
            Sign = keystore.Session.KeySign,
            GcmCalc = encrypted,
            Type = 1
        };
        
        return encryptPacket.Serialize();
    }
}