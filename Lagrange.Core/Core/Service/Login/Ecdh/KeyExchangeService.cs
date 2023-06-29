using System.Security.Cryptography;
using Lagrange.Core.Common;
using Lagrange.Core.Core.Event.Protocol;
using Lagrange.Core.Core.Event.Protocol.Login.Ecdh;
using Lagrange.Core.Core.Packets;
using Lagrange.Core.Core.Packets.Login.Ecdh;
using Lagrange.Core.Core.Packets.Login.Ecdh.Plain;
using Lagrange.Core.Core.Service.Abstraction;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Crypto;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Core.Service.Login.Ecdh;

[EventSubscribe(typeof(KeyExchangeEvent))]
[Service("trpc.login.ecdh.EcdhService.SsoKeyExchange")]
internal class KeyExchangeService : BaseService<KeyExchangeEvent>
{
    private const string GcmCalc2Key = "e2733bf403149913cbf80c7a95168bd4ca6935ee53cd39764beebe2e007e3aee";

    protected override bool Build(KeyExchangeEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = BuildPacket(keystore, device);
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream.ToArray());
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(SsoPacket input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out KeyExchangeEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = input.Payload.ReadBytes(BinaryPacket.Prefix.Uint32 | BinaryPacket.Prefix.WithPrefix);
        var response = Serializer.Deserialize<SsoKeyExchangeResponse>(payload.AsSpan());

        var shareKey = keystore.PrimeImpl.GenerateShared(response.PublicKey, false);
        var gcmDecrypted = new AesGcmImpl().Decrypt(response.GcmEncrypted, shareKey);
        var decrypted = Serializer.Deserialize<SsoKeyExchangeDecrypted>(gcmDecrypted.AsSpan());

        keystore.Session.ExchangeKey = decrypted.GcmKey;
        keystore.Session.KeySign = decrypted.Sign;
        output = KeyExchangeEvent.Result();
        
        extraEvents = null;
        return true;
    }

    private static SsoKeyExchange BuildPacket(BotKeystore keystore, BotDeviceInfo deviceInfo)
    {
        using var stream = new MemoryStream();
        var plain1 = new SsoKeyExchangePlain
        {
            Uin = keystore.Uin.ToString(),
            Guid = deviceInfo.Guid.ToByteArray()
        };
        Serializer.Serialize(stream, plain1);
        var gcmCalc1 = new AesGcmImpl().Encrypt(stream.ToArray(), keystore.PrimeImpl.ShareKey);

        var timestamp = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var plain2 = new SsoKeyExchangePlain2
        {
            PublicKey = keystore.PrimeImpl.GetPublicKey(false),
            Type = 1,
            EncryptedGcm = gcmCalc1,
            Const = 0,
            Timestamp = timestamp
        };
        var stream2 = BinarySerializer.Serialize(plain2);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(stream2.ToArray()) ?? throw new InvalidOperationException();
        var gcmCalc2 = new AesGcmImpl().Encrypt(hash, GcmCalc2Key.UnHex());

        return new SsoKeyExchange
        {
            PubKey = keystore.PrimeImpl.GetPublicKey(false),
            Type = 1,
            GcmCalc1 = gcmCalc1,
            Timestamp = timestamp,
            GcmCalc2 = gcmCalc2
        };
    }
}