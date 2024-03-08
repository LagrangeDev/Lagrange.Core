using System.Security.Cryptography;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Login;
using Lagrange.Core.Internal.Packets.Login.Ecdh;
using Lagrange.Core.Internal.Packets.Login.Ecdh.Plain;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Crypto;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Login;

[EventSubscribe(typeof(KeyExchangeEvent))]
[Service("trpc.login.ecdh.EcdhService.SsoKeyExchange")]
internal class KeyExchangeService : BaseService<KeyExchangeEvent>
{
    private const string GcmCalc2Key = "e2733bf403149913cbf80c7a95168bd4ca6935ee53cd39764beebe2e007e3aee";

    private static readonly byte[] PubKey = new byte[] // From NTQQ Binary, by hook
    {
        0x04,
        0x9D, 0x14, 0x23, 0x33, 0x27, 0x35, 0x98, 0x0E,
        0xDA, 0xBE, 0x7E, 0x9E, 0xA4, 0x51, 0xB3, 0x39,
        0x5B, 0x6F, 0x35, 0x25, 0x0D, 0xB8, 0xFC, 0x56,
        0xF2, 0x58, 0x89, 0xF6, 0x28, 0xCB, 0xAE, 0x3E,
        0x8E, 0x73, 0x07, 0x79, 0x14, 0x07, 0x1E, 0xEE,
        0xBC, 0x10, 0x8F, 0x4E, 0x01, 0x70, 0x05, 0x77,
        0x92, 0xBB, 0x17, 0xAA, 0x30, 0x3A, 0xF6, 0x52,
        0x31, 0x3D, 0x17, 0xC1, 0xAC, 0x81, 0x5E, 0x79
    };

    private EcdhImpl EcdhImpl = new EcdhImpl(EcdhImpl.CryptMethod.Prime256V1);

    protected override bool Build(KeyExchangeEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        using var stream = new MemoryStream();
        var plain1 = new SsoKeyExchangePlain
        {
            Uin = keystore.Uin.ToString(),
            Guid = device.System.Guid.ToByteArray()
        };
        Serializer.Serialize(stream, plain1);

        EcdhImpl.GenerateShared(PubKey, false);
        var gcmCalc1 = new AesGcmImpl().Encrypt(stream.ToArray(), EcdhImpl.ShareKey);

        var timestamp = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var plain2 = new SsoKeyExchangePlain2
        {
            PublicKey = EcdhImpl.GetPublicKey(false),
            Type = 1,
            EncryptedGcm = gcmCalc1,
            Const = 0,
            Timestamp = timestamp
        };
        var stream2 = BinarySerializer.Serialize(plain2);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(stream2.ToArray());
        var gcmCalc2 = new AesGcmImpl().Encrypt(hash, GcmCalc2Key.UnHex());

        var packet = new SsoKeyExchange
        {
            PubKey = EcdhImpl.GetPublicKey(false),
            Type = 1,
            GcmCalc1 = gcmCalc1,
            Timestamp = timestamp,
            GcmCalc2 = gcmCalc2
        };

        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out KeyExchangeEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var response = Serializer.Deserialize<SsoKeyExchangeResponse>(input.AsSpan());

        var shareKey = EcdhImpl.GenerateShared(response.PublicKey, false);
        var gcmDecrypted = new AesGcmImpl().Decrypt(response.GcmEncrypted, shareKey);
        var decrypted = Serializer.Deserialize<SsoKeyExchangeDecrypted>(gcmDecrypted.AsSpan());

        keystore.Session.ExchangeKey = decrypted.GcmKey;
        keystore.Session.KeySign = decrypted.Sign;
        output = KeyExchangeEvent.Result();

        extraEvents = null;
        return true;
    }
}