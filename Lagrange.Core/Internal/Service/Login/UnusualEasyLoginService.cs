using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Login;
using Lagrange.Core.Internal.Packets.Login.NTLogin;
using Lagrange.Core.Internal.Packets.Login.NTLogin.Plain;
using Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Body;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Crypto;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Login;

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

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out UnusualEasyLoginEvent output, out List<ProtocolEvent>? extraEvents)
    {
        if (keystore.Session.ExchangeKey == null) throw new InvalidOperationException("ExchangeKey is null");
        
        var encrypted = Serializer.Deserialize<SsoNTLoginEncryptedData>(input);

        if (encrypted.GcmCalc != null)
        {
            var decrypted = new AesGcmImpl().Decrypt(encrypted.GcmCalc, keystore.Session.ExchangeKey);
            var response = Serializer.Deserialize<SsoNTLoginBase<SsoNTLoginResponse>>(decrypted.AsSpan());
            var body = response.Body;
            
            if (response.Header?.Error != null || body is not { Credentials: not null })
            {
                output = UnusualEasyLoginEvent.Result((int)(response.Header?.Error?.ErrorCode ?? 1));
            }
            else
            {
                keystore.Session.Tgt = body.Credentials.Tgt;
                keystore.Session.D2 = body.Credentials.D2;
                keystore.Session.D2Key = body.Credentials.D2Key;
                keystore.Session.TempPassword = body.Credentials.TempPassword;
                keystore.Session.SessionDate = DateTime.Now;

                output = UnusualEasyLoginEvent.Result(0);
            }
        }
        else
        {
            output = UnusualEasyLoginEvent.Result(1);
        }

        extraEvents = null;
        return true;
    }
}