using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Login;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Login.NTLogin;
using Lagrange.Core.Core.Packets.Login.NTLogin.Plain;
using Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Body;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Crypto;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Login;

[EventSubscribe(typeof(UnusualEasyLoginEvent))]
[Service("trpc.login.ecdh.EcdhService.SsoNTLoginEasyLoginUnusualDevice")]
internal class UnusualEasyLoginService : BaseService<UnusualEasyLoginEvent>
{
    protected override bool Build(UnusualEasyLoginEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        if (keystore.Session.TempPassword == null) throw new InvalidOperationException("TempPassword is null");

        output = SsoNTLoginCommon.BuildNTLoginPacket(keystore, appInfo, device, keystore.Session.TempPassword);
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out UnusualEasyLoginEvent output, out List<ProtocolEvent>? extraEvents)
    {
        if (keystore.Session.ExchangeKey == null) throw new InvalidOperationException("ExchangeKey is null");
        
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var encrypted = Serializer.Deserialize<SsoNTLoginEncryptedData>(payload.AsSpan());

        if (encrypted.GcmCalc != null)
        {
            var decrypted = new AesGcmImpl().Decrypt(encrypted.GcmCalc, keystore.Session.ExchangeKey);
            var response = Serializer.Deserialize<SsoNTLoginBase<SsoNTLoginResponse>>(decrypted.AsSpan());
            var body = response.Body;

            if (body != null && response.Header?.Error == null && body.Credentials != null)
            {
                keystore.Session.Tgt = body.Credentials.Tgt;
                keystore.Session.D2 = body.Credentials.D2;
                keystore.Session.D2Key = body.Credentials.D2Key;
                keystore.Session.TempPassword = body.Credentials.TempPassword;

                output = UnusualEasyLoginEvent.Result(true);
            }
            else
            {
                output = UnusualEasyLoginEvent.Result(false);
            }
        }
        else
        {
            output = UnusualEasyLoginEvent.Result(false);
        }

        extraEvents = null;
        return true;
    }
}