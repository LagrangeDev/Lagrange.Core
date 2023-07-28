using Lagrange.Core.Common;
using Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Universal;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Crypto;
using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Body;

// ReSharper disable InconsistentNaming

internal static class SsoNTLoginCommon
{
    public static BinaryPacket BuildNTLoginPacket(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, byte[] body)
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
                    Type = 1,
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
            Body = new SsoNTLoginEasyLogin { TempPassword = body }
        };
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        var encrypted = new AesGcmImpl().Encrypt(stream.ToArray(), keystore.Session.ExchangeKey);

        var encryptPacket = new SsoNTLoginEncryptedData
        {
            Sign = keystore.Session.KeySign,
            GcmCalc = encrypted,
            Type = 1
        };
        
        using var encryptStream = new MemoryStream();
        Serializer.Serialize(encryptStream, encryptPacket);
        return new BinaryPacket(encryptStream.ToArray());
    }
}