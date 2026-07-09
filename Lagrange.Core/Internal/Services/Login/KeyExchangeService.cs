using System.Security.Cryptography;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Cryptography;

namespace Lagrange.Core.Internal.Services.Login;

[EventSubscribe<KeyExchangeEventReq>(Protocols.All)]
[Service("trpc.login.ecdh.EcdhService.SsoKeyExchange", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class KeyExchangeService : BaseService<KeyExchangeEventReq, KeyExchangeEventResp>
{
    private static readonly byte[] VerifyHashKey =
    [
        0xe2, 0x73, 0x3b, 0xf4, 0x03, 0x14, 0x99, 0x13, 0xcb, 0xf8, 0x0c, 0x7a, 0x95, 0x16, 0x8b, 0xd4, 
        0xca, 0x69, 0x35, 0xee, 0x53, 0xcd, 0x39, 0x76, 0x4b, 0xee, 0xbe, 0x2e, 0x00, 0x7e, 0x3a, 0xee
    ];

    private static readonly byte[] ServerPublicKey =
    [
        0x04,
        0x9D, 0x14, 0x23, 0x33, 0x27, 0x35, 0x98, 0x0E, 0xDA, 0xBE, 0x7E, 0x9E, 0xA4, 0x51, 0xB3, 0x39,
        0x5B, 0x6F, 0x35, 0x25, 0x0D, 0xB8, 0xFC, 0x56, 0xF2, 0x58, 0x89, 0xF6, 0x28, 0xCB, 0xAE, 0x3E,
        0x8E, 0x73, 0x07, 0x79, 0x14, 0x07, 0x1E, 0xEE, 0xBC, 0x10, 0x8F, 0x4E, 0x01, 0x70, 0x05, 0x77,
        0x92, 0xBB, 0x17, 0xAA, 0x30, 0x3A, 0xF6, 0x52, 0x31, 0x3D, 0x17, 0xC1, 0xAC, 0x81, 0x5E, 0x79
    ];
    
    protected override ValueTask<ReadOnlyMemory<byte>> Build(KeyExchangeEventReq input, BotContext context)
    {
        var publicKey = context.Keystore.Prime256V1.PackPublic(false);
        var shareKey = context.Keystore.Prime256V1.KeyExchange(ServerPublicKey, false);
        uint timestamp = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        var secret = new KeyExchangeRequestBuf
        {
            Uin = context.Keystore.Uin.ToString(),
            Guid = context.Keystore.Guid
        };
        var secretBuf = AesGcmProvider.Encrypt(ProtoHelper.Serialize(secret).Span, shareKey);
        
        var verifyPacket = new BinaryPacket(stackalloc byte[100]);
        verifyPacket.Write(publicKey);
        verifyPacket.Write(1); // type
        verifyPacket.Write(secretBuf);
        verifyPacket.Write(0);
        verifyPacket.Write(timestamp);
        var verifyHash = AesGcmProvider.Encrypt(SHA256.HashData(verifyPacket.CreateReadOnlySpan()), VerifyHashKey);

        var request = new KeyExchangeRequest
        {
            PublicKey = publicKey,
            Type = 1,
            Secret = secretBuf,
            Timestamp = timestamp,
            VerifyHash = verifyHash
        };
        
        return new ValueTask<ReadOnlyMemory<byte>>(ProtoHelper.Serialize(request));
    }

    protected override ValueTask<KeyExchangeEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var response = ProtoHelper.Deserialize<KeyExchangeResponse>(input.Span);
        var shareKey = context.Keystore.Prime256V1.KeyExchange(response.PublicKey, false);
        var secret = ProtoHelper.Deserialize<KeyExchangeResponseSecret>(AesGcmProvider.Decrypt(response.Secret, shareKey));

        return new ValueTask<KeyExchangeEventResp>(new KeyExchangeEventResp(secret.SessionTicket, secret.SessionKey));
    }
}