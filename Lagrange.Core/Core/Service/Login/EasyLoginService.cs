using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Login;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Login.NTLogin;
using Lagrange.Core.Core.Packets.Login.NTLogin.Plain;
using Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Body;
using Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Universal;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Crypto;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Login;

[EventSubscribe(typeof(EasyLoginEvent))]
[Service("trpc.login.ecdh.EcdhService.SsoNTLoginEasyLogin")]
internal class EasyLoginService : BaseService<EasyLoginEvent>
{
    protected override bool Build(EasyLoginEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        if (keystore.Session.TempPassword == null) throw new InvalidOperationException("TempPassword is null");

        output = SsoNTLoginCommon.BuildNTLoginPacket(keystore, appInfo, device, keystore.Session.TempPassword);
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out EasyLoginEvent output, out List<ProtocolEvent>? extraEvents)
    {
        if (keystore.Session.ExchangeKey == null) throw new InvalidOperationException("ExchangeKey is null");
        
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var encrypted = Serializer.Deserialize<SsoNTLoginEncryptedData>(payload.AsSpan());

        if (encrypted.GcmCalc != null)
        {
            var decrypted = new AesGcmImpl().Decrypt(encrypted.GcmCalc, keystore.Session.ExchangeKey);
            var response = Serializer.Deserialize<SsoNTLoginBase<SsoNTLoginResponse>>(decrypted.AsSpan()).Body;

            if (response != null)
            {
                if (response.Unusual != null || response.Credentials == null)
                {
                    keystore.Session.UnusualSign = response.Unusual?.Sig;
                }
                else
                {
                    keystore.Session.Tgt = response.Credentials.Tgt;
                    keystore.Session.D2 = response.Credentials.D2;
                    keystore.Session.D2Key = response.Credentials.D2Key;
                    keystore.Session.TempPassword = response.Credentials.TempPassword;
                }

                output = EasyLoginEvent.Result(true, response.Unusual != null);
            }
            else
            {
                output = EasyLoginEvent.Result(false);
            }
        }
        else
        {
            output = EasyLoginEvent.Result(false);
        }

        extraEvents = null;
        return true;
    }
}